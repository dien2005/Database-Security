-- ============================================================
-- DATA REDACTION + FINE-GRAINED AUDITING (FGA)
-- GIAI ĐOẠN 5
-- ============================================================
-- Chạy SAU:
--   + SYS_SETUP.sql
--   + HR_ADMIN_DDL_INSERT.sql
--   + USER_DATA.sql
--   + RBAC.sql
--   + adduser.sql
--   + VPD_SETUP_FIXED.sql
--   + OLS_SETUP.sql
-- Mục tiêu:
--   - Bổ sung lớp bảo mật cuối cùng sau RBAC + VPD + OLS
--   - Không thay đổi bất kỳ object nào của các giai đoạn trước
--   - Chỉ bổ sung Data Redaction và Fine-Grained Auditing
-- ============================================================

-- ============================================================
-- BƯỚC 1
-- TẠO WRAPPER ROLES CHO DATA REDACTION
-- Chạy bằng:
--      SYS AS SYSDBA
-- Mục đích:
-- Không để DBMS_REDACT phụ thuộc trực tiếp vào các role nghiệp vụ
-- (HR_MANAGER_ROLE, FIN_STAFF_ROLE,...)
-- Thay vào đó tạo một lớp Wrapper Role.
-- Sau này nếu thay đổi chính sách chỉ cần GRANT Wrapper Role,
-- không phải DROP/CREATE lại Redaction Policy.
-- ============================================================

---------------------------------------------------------------
-- 1.1 Xóa Wrapper Role cũ (nếu tồn tại)
---------------------------------------------------------------
BEGIN
    FOR r IN (
        SELECT role FROM dba_roles
        WHERE role IN ('PAYROLL_VIEW_ROLE', 'BONUS_VIEW_ROLE', 'HR_CONTACT_VIEW_ROLE')
    )
    LOOP
        EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
    END LOOP;
END;
/

---------------------------------------------------------------
-- 1.2 Tạo Wrapper Roles
---------------------------------------------------------------
-- Được xem đầy đủ thông tin lương cơ bản
CREATE ROLE payroll_view_role;
-- Được xem tiền thưởng
CREATE ROLE bonus_view_role;
-- Được xem đầy đủ thông tin liên hệ
CREATE ROLE hr_contact_view_role;


---------------------------------------------------------------
-- 1.3 Mapping Wrapper Role
---------------------------------------------------------------
---------------------------------------------------------------
-- PAYROLL_VIEW_ROLE
-- Những role này được nhìn thấy BASE_SALARY thật
---------------------------------------------------------------
GRANT payroll_view_role TO hr_manager_role;
GRANT payroll_view_role TO hr_staff_role;
GRANT payroll_view_role TO dept_manager_role;
GRANT payroll_view_role TO fin_staff_role;

---------------------------------------------------------------
-- BONUS_VIEW_ROLE
-- BONUS nhạy cảm hơn BASE_SALARY
-- Chỉ HR và Finance mới được xem.
---------------------------------------------------------------
GRANT bonus_view_role TO hr_manager_role;
GRANT bonus_view_role TO hr_staff_role;
GRANT bonus_view_role TO fin_staff_role;

---------------------------------------------------------------
-- HR_CONTACT_VIEW_ROLE
-- Được xem đầy đủ số điện thoại.
---------------------------------------------------------------
GRANT hr_contact_view_role TO hr_manager_role;
GRANT hr_contact_view_role TO hr_staff_role;


---------------------------------------------------------------
-- 1.4 Verify Wrapper Role
---------------------------------------------------------------
COLUMN ROLE FORMAT A25
COLUMN GRANTED_ROLE FORMAT A30
SELECT role FROM dba_roles
WHERE role IN ('PAYROLL_VIEW_ROLE', 'BONUS_VIEW_ROLE', 'HR_CONTACT_VIEW_ROLE')
ORDER BY role;

-- WRAPPER ROLE HIERARCHY
SELECT grantee, granted_role FROM dba_role_privs
WHERE granted_role IN ('PAYROLL_VIEW_ROLE', 'BONUS_VIEW_ROLE', 'HR_CONTACT_VIEW_ROLE')
ORDER BY granted_role, grantee;
COMMIT;




-- ============================================================
-- BƯỚC 2
-- DATA REDACTION - BASE_SALARY
-- Chạy bằng: SYS AS SYSDBA
--
-- Mục đích:
-- Che cột BASE_SALARY đối với những user KHÔNG thuộc PAYROLL_VIEW_ROLE.
-- Các role HR, Finance, Department Manager vẫn nhìn thấy giá trị thật.
-- ============================================================

---------------------------------------------------------------
-- 2.1 Xóa policy cũ (nếu chạy lại script)
---------------------------------------------------------------
BEGIN
    DBMS_REDACT.DROP_POLICY
    (
        object_schema => 'HR_ADMIN',
        object_name   => 'PAYROLL',
        policy_name   => 'PAYROLL_BASE_SALARY_POLICY'
    );
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

---------------------------------------------------------------
-- 2.2 Tạo Policy cho BASE_SALARY
---------------------------------------------------------------
BEGIN
    DBMS_REDACT.ADD_POLICY
    (
        object_schema      => 'HR_ADMIN',
        object_name        => 'PAYROLL',
        policy_name        => 'PAYROLL_BASE_SALARY_POLICY',
        policy_description => 'Hide BASE_SALARY for unauthorized users',
        column_name        => 'BASE_SALARY',
        function_type      => DBMS_REDACT.FULL,
        expression         =>
        q'[SYS_CONTEXT('SYS_SESSION_ROLES', 'PAYROLL_VIEW_ROLE') = 'FALSE']'
    );
END;
/


---------------------------------------------------------------
-- 2.3 Verify
---------------------------------------------------------------
COLUMN OBJECT_NAME FORMAT A15
COLUMN COLUMN_NAME FORMAT A20
SELECT
       object_owner,
       object_name,
       column_name,
       function_type
FROM redaction_columns
WHERE object_owner='HR_ADMIN'
AND object_name='PAYROLL';

COMMIT;

-- Khong the ap dung data redaction tren 2 cot cung luc tren cung 1 bang


-- ============================================================
-- BƯỚC 3
-- DATA REDACTION - EMPLOYEES.PHONE
-- Chạy bằng: SYS AS SYSDBA
--
-- Mục đích:
-- Che số điện thoại của nhân viên đối với những user KHÔNG thuộc HR_CONTACT_VIEW_ROLE.
-- HR Manager và HR Staff vẫn nhìn thấy đầy đủ.
-- ============================================================
---------------------------------------------------------------
-- 3.1 Xóa Policy cũ (nếu có)
---------------------------------------------------------------
BEGIN
    DBMS_REDACT.DROP_POLICY
    (
        object_schema => 'HR_ADMIN',
        object_name   => 'EMPLOYEES',
        policy_name   => 'EMPLOYEE_PHONE_POLICY'
    );
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

---------------------------------------------------------------
-- 3.2 Tạo Policy cho PHONE
---------------------------------------------------------------
BEGIN
    DBMS_REDACT.ADD_POLICY
    (
        object_schema      => 'HR_ADMIN',
        object_name        => 'EMPLOYEES',
        policy_name        => 'EMPLOYEE_PHONE_POLICY',
        policy_description => 'Hide employee phone number',
        column_name        => 'PHONE',
        function_type      => DBMS_REDACT.FULL,
        expression         =>
        q'[SYS_CONTEXT('SYS_SESSION_ROLES','HR_CONTACT_VIEW_ROLE')='FALSE']'
    );
END;
/

---------------------------------------------------------------
-- 3.3 Verify
---------------------------------------------------------------
COLUMN OBJECT_NAME FORMAT A15
COLUMN COLUMN_NAME FORMAT A20

SELECT
       object_owner,
       object_name,
       column_name,
       function_type
FROM redaction_columns
WHERE object_owner='HR_ADMIN'
AND object_name='EMPLOYEES';

COMMIT;



-- ============================================================
-- BƯỚC 4: KIEM TRA CATALOG REDACTION_POLICIES
-- Chạy bằng: HR_ADMIN hoac SYS
-- ============================================================
SELECT object_owner,
       object_name,
       policy_name,
       expression
FROM redaction_policies
WHERE object_owner='HR_ADMIN'
ORDER BY object_name, policy_name;



-- ============================================================
-- BƯỚC 5: THIẾT LẬP FINE-GRAINED AUDITING (FGA)
-- Chạy bằng: SYS AS SYSDBA
-- Mục đích: Ghi lại các lần truy cập vào dữ liệu nhạy cảm.
-- Khác với VPD: VPD quyết định được xem hay không.
-- Khác với Data Redaction: Redaction che giá trị dữ liệu.
-- FGA không chặn truy cập.
-- FGA chỉ ghi nhận:
--      - Ai truy cập
--      - Truy cập lúc nào
--      - Truy cập bảng nào
--      - Thực hiện câu SQL gì
-- Trong hệ thống HR này audit: BASE_SALARY, PHONE
-- ============================================================
---------------------------------------------------------------
-- 5.1 Xóa FGA Policy cũ (nếu có)
---------------------------------------------------------------
BEGIN
    DBMS_FGA.DROP_POLICY(
        object_schema => 'HR_ADMIN',
        object_name   => 'PAYROLL',
        policy_name   => 'FGA_PAYROLL_SALARY_ACCESS'
    );
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
    DBMS_FGA.DROP_POLICY(
        object_schema => 'HR_ADMIN',
        object_name   => 'EMPLOYEES',
        policy_name   => 'FGA_EMPLOYEE_PHONE_ACCESS'
    );
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

---------------------------------------------------------------
-- 5.2 Audit truy cập BASE_SALARY
---------------------------------------------------------------
BEGIN
    DBMS_FGA.ADD_POLICY
    (
        object_schema      => 'HR_ADMIN',
        object_name        => 'PAYROLL',
        policy_name        => 'FGA_PAYROLL_SALARY_ACCESS',
        audit_column       => 'BASE_SALARY',
        statement_types    => 'SELECT',
        audit_condition    =>  NULL,
        audit_trail        =>  DBMS_FGA.DB + DBMS_FGA.EXTENDED
    );
END;
/

---------------------------------------------------------------
-- 5.3 Audit truy cập PHONE
---------------------------------------------------------------
BEGIN
    DBMS_FGA.ADD_POLICY
    (
        object_schema      => 'HR_ADMIN',
        object_name        => 'EMPLOYEES',
        policy_name        => 'FGA_EMPLOYEE_PHONE_ACCESS',
        audit_column       => 'PHONE',
        statement_types    => 'SELECT',
        audit_condition    =>  NULL,
        audit_trail        =>  DBMS_FGA.DB + DBMS_FGA.EXTENDED
    );
END;
/

---------------------------------------------------------------
-- 5.4 Verify FGA Policy
---------------------------------------------------------------
COLUMN OBJECT_SCHEMA FORMAT A15
COLUMN OBJECT_NAME FORMAT A20
COLUMN POLICY_NAME FORMAT A35
COLUMN ENABLED FORMAT A8

SELECT
    object_schema,
    object_name,
    policy_name,
    enabled
FROM dba_audit_policies
WHERE object_schema='HR_ADMIN'
ORDER BY object_name;

COMMIT;



-- ============================================================
-- BƯỚC 6: TẠO BẢNG AUDIT RIÊNG CHO HỆ THỐNG HR
-- Chạy bằng: HR_ADMIN
--
-- Mục đích:
-- Oracle vẫn lưu log tại UNIFIED_AUDIT_TRAIL.
-- Hệ thống HR sử dụng bảng AUDIT_ACCESS_LOG để quản lý,
-- tìm kiếm và hiển thị lịch sử truy cập dữ liệu nhạy cảm.
-- ============================================================

---------------------------------------------------------------
-- 6.1 Drop bảng cũ (nếu tồn tại)
---------------------------------------------------------------
BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE audit_access_log CASCADE CONSTRAINTS';
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END;
/

---------------------------------------------------------------
-- 6.2 Tạo bảng
---------------------------------------------------------------
CREATE TABLE audit_access_log
(
    audit_id        NUMBER GENERATED BY DEFAULT AS IDENTITY
                    PRIMARY KEY,
    event_time      TIMESTAMP       NOT NULL,
    db_user         VARCHAR2(128)   NOT NULL,
    object_schema   VARCHAR2(128),
    object_name     VARCHAR2(128),
    policy_name     VARCHAR2(128),
    action_name     VARCHAR2(30),
    sql_text        CLOB,
    sync_time       TIMESTAMP DEFAULT SYSTIMESTAMP
);
COMMENT ON TABLE audit_access_log IS
'Business audit log for sensitive data access';
COMMENT ON COLUMN audit_access_log.event_time IS
'Time recorded by Oracle Unified Audit';
COMMENT ON COLUMN audit_access_log.sync_time IS
'Time synchronized into HR system';
COMMIT;


-- 6.3 Verify
DESC audit_access_log;

SELECT *
FROM audit_access_log;

-- chạy bằng SYS:
GRANT SELECT ON AUDSYS.UNIFIED_AUDIT_TRAIL TO HR_ADMIN;


-- ============================================================
-- BƯỚC 7: ĐỒNG BỘ FGA VÀO BẢNG AUDIT RIÊNG CỦA HỆ THỐNG
-- Chạy bằng: HR_ADMIN
-- Mục đích: Đọc các log FGA trong UNIFIED_AUDIT_TRAIL và ghi sang HR_ADMIN.AUDIT_ACCESS_LOG.
--
-- Chỉ đồng bộ những policy của đồ án:
--      + FGA_PAYROLL_SALARY_ACCESS
--      + FGA_EMPLOYEE_PHONE_ACCESS
-- ============================================================
---------------------------------------------------------------
-- 7.1 Xóa procedure cũ
---------------------------------------------------------------
BEGIN
    EXECUTE IMMEDIATE
    'DROP PROCEDURE HR_ADMIN.SYNC_FGA_AUDIT_LOG';
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END;
/

---------------------------------------------------------------
-- 7.2 Procedure đồng bộ
---------------------------------------------------------------
CREATE OR REPLACE PROCEDURE HR_ADMIN.SYNC_FGA_AUDIT_LOG
AS
BEGIN
    INSERT INTO HR_ADMIN.AUDIT_ACCESS_LOG
    (
        event_time,
        db_user,
        object_schema,
        object_name,
        policy_name,
        action_name,
        sql_text
    )
    SELECT
        ua.event_timestamp,
        ua.dbusername,
        ua.object_schema,
        ua.object_name,
        ua.fga_policy_name,
        ua.action_name,
        ua.sql_text
    FROM AUDSYS.UNIFIED_AUDIT_TRAIL ua
    WHERE ua.fga_policy_name IN
    (
        'FGA_PAYROLL_SALARY_ACCESS',
        'FGA_EMPLOYEE_PHONE_ACCESS'
    )
    AND ua.action_name='SELECT'
    AND ua.object_schema='HR_ADMIN'
    AND ua.event_timestamp >
    (
        SELECT NVL(MAX(event_time),
                   TO_TIMESTAMP('2000-01-01','YYYY-MM-DD'))
        FROM HR_ADMIN.AUDIT_ACCESS_LOG
    );
    COMMIT;
END;
/



-- Truy cập dữ liệu nhạy cảm
CONNECT minhlh/minhlh;
SELECT employee_id, phone
FROM hr_admin.employees;

SELECT employee_id, base_salary
FROM hr_admin.payroll;


-- Đồng bộ log
CONNECT hr_admin/hr_admin;
BEGIN
    sync_fga_audit_log;
END;
/



-- Kiểm tra bảng audit của hệ thống
SELECT audit_id,
       event_time,
       db_user,
       object_name,
       policy_name,
       action_name
FROM audit_access_log
ORDER BY audit_id DESC;


-- Kiểm tra SQL truy cập
SELECT audit_id,
       db_user,
       sql_text
FROM audit_access_log
ORDER BY audit_id DESC;

