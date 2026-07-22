-- ============================================================
-- ORACLE LABEL SECURITY (OLS) SETUP
-- Giai đoạn 4 — Chạy SAU: HR_ADMIN_DDL_INSERT.sql, RBAC.sql,
--                         VPD_SETUP_FIXED.sql, adduser.sql
--
-- Thiết kế :
--   - Áp OLS cho 2 bảng: EMPLOYEES, PAYROLL
--   - LEVEL  = mức độ nhạy cảm của BẢNG (chiều dọc, có thứ bậc)
--        PUBLIC(100) < INTERNAL(200) < CONFIDENTIAL(300) < SECRET(400)
--        EMPLOYEES -> INTERNAL | PAYROLL -> CONFIDENTIAL
--   - GROUP  = PHÒNG BAN (chiều ngang), phân cấp cha-con
--     trùng parent_dept_id:
--        BGD (gốc)
--         ├── HRD
--         ├── FIN
--         └── ITD
--   - KHÔNG dùng COMPARTMENT
--   - OLS + VPD + RBAC là AND với nhau. OLS chỉ đóng vai trò
--     lớp phòng thủ thứ 2 (coarse-grained), "đúng hàng nào"
--     vẫn do VPD quyết định -> label user phải đủ permissive
--     cho mọi role mà RBAC/VPD đã cho phép, tránh chặn nhầm.
-- ============================================================


-- ============================================================
-- BƯỚC 0: ENABLE OLS OPTION (làm 1 lần, ngoài SQL script)
-- ============================================================
-- Chạy ở OS, quyền root/oracle owner, KHÔNG chạy trong SQL*Plus:
--
--   sqlplus / as sysdba
--   SHUTDOWN IMMEDIATE;
--   EXIT;
--   cd $ORACLE_HOME/bin
--   ./chopt enable lbac
--   sqlplus / as sysdba
--   STARTUP;
--
-- Nếu LBACSYS chưa có đủ package (kiểm tra dba_objects), chạy
-- catalog script để dựng schema LBACSYS (chạy bằng SYS):
--
--   @?/rdbms/admin/catlbac.sql
--
-- Lưu ý: Oracle XE KHÔNG hỗ trợ OLS (chỉ Enterprise Edition).
-- Lưu ý: tên package thật có thể là LBAC_xxx, được truy cập qua
-- synonym công khai SA_xxx (VD: SA_POLICY_ADMIN -> synonym trỏ
-- tới LBACSYS.LBAC_POLICY_ADMIN) - dùng đúng tên SA_xxx là đủ.
-- ============================================================


-- ============================================================
-- BƯỚC 1: TẠO USER SEC_ADMIN — chuyên trách quản trị OLS
-- Chạy bằng: SYS AS SYSDBA
-- ============================================================

BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'SEC_ADMIN') LOOP
        EXECUTE IMMEDIATE 'DROP USER sec_admin CASCADE';
    END LOOP;
END;
/
CREATE USER sec_admin IDENTIFIED BY sec_admin
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;   
GRANT CREATE SESSION TO sec_admin;

-- chạy bằng SYSDBA để cấp quyền OLS cho SEC_ADMIN, KHÔNG grant cho HR_ADMIN
-- Package dùng để tạo ra các thành phần của nhãn (level, group, compartment)
GRANT EXECUTE ON sa_components   TO sec_admin;

-- Package dùng để tạo các nhãn (label)
GRANT EXECUTE ON sa_label_admin  TO sec_admin;

-- Package dùng để gán chính sách cho các bảng/schema
GRANT EXECUTE ON sa_policy_admin TO sec_admin;

-- Package dùng để gán label/privilege cho từng user
GRANT EXECUTE ON sa_user_admin   TO sec_admin;

-- KHÔNG grant sa_sysdba cho sec_admin hay bất kỳ ai khác:
-- CREATE_POLICY/DROP_POLICY chỉ chạy được bằng phiên SYSDBA thật


-- ============================================================
-- BƯỚC 2: TẠO POLICY
-- Chạy bằng: SYS AS SYSDBA (bắt buộc, không thể delegate)
-- ============================================================

BEGIN
   SA_SYSDBA.CREATE_POLICY(
      policy_name     => 'HR_OLS_POLICY',
      column_name     => 'OLS_LABEL',
      default_options => 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL, HIDE'
   );
END;
/

GRANT HR_OLS_POLICY_DBA TO sec_admin;

-- ============================================================
-- BƯỚC 3: TẠO LEVELS — khớp data_classification.ols_label
-- Chạy bằng: SEC_ADMIN
-- ============================================================
SET ROLE HR_OLS_POLICY_DBA;

BEGIN
   SA_COMPONENTS.CREATE_LEVEL('HR_OLS_POLICY', 100, 'PUB',  'PUBLIC');
   SA_COMPONENTS.CREATE_LEVEL('HR_OLS_POLICY', 200, 'INT',  'INTERNAL');
   SA_COMPONENTS.CREATE_LEVEL('HR_OLS_POLICY', 300, 'CONF', 'CONFIDENTIAL');
   SA_COMPONENTS.CREATE_LEVEL('HR_OLS_POLICY', 400, 'SEC',  'SECRET');
END;
/


-- ============================================================
-- BƯỚC 4: TẠO GROUPS — khớp phòng ban, phân cấp cha-con
-- Chạy bằng: SEC_ADMIN
-- ============================================================

BEGIN
   SA_COMPONENTS.CREATE_GROUP('HR_OLS_POLICY', 100, 'BGD', 'Ban Giam Doc');
   SA_COMPONENTS.CREATE_GROUP('HR_OLS_POLICY', 200, 'HRD', 'Phong Nhan Su', 'BGD');
   SA_COMPONENTS.CREATE_GROUP('HR_OLS_POLICY', 300, 'FIN', 'Phong Ke Toan', 'BGD');
   SA_COMPONENTS.CREATE_GROUP('HR_OLS_POLICY', 400, 'ITD', 'Phong IT',      'BGD');
END;
/


-- ============================================================
-- BƯỚC 5: ÁP POLICY VÀO BẢNG EMPLOYEES/PAYROLL
-- Chạy bằng: SEC_ADMIN
-- (SEC_ADMIN không phải owner của EMPLOYEES/PAYROLL, nhưng
--  APPLY_TABLE_POLICY cho phép chỉ định schema_name tường minh)
-- ============================================================

BEGIN
   SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
      policy_name    => 'HR_OLS_POLICY',
      schema_name    => 'HR_ADMIN',
      table_name     => 'EMPLOYEES',
      table_options  => 'NO_CONTROL'
   );

   SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
      policy_name    => 'HR_OLS_POLICY',
      schema_name    => 'HR_ADMIN',
      table_name     => 'PAYROLL',
      table_options  => 'NO_CONTROL'
   );
END;
/


-- ============================================================
-- BƯỚC 6: CẤP QUYỀN FULL CHO HR_ADMIN
-- Chạy bằng: SEC_ADMIN
-- (HR_ADMIN cần bypass OLS để tự chạy UPDATE gán label hàng
--  loạt cho data của chính mình ở Bước 8 - nhưng quyền này do
--  SEC_ADMIN cấp, HR_ADMIN không tự cấp cho mình)
-- ============================================================

BEGIN
   SA_USER_ADMIN.SET_USER_PRIVS('HR_OLS_POLICY', 'HR_ADMIN', 'FULL');
END;
/

BEGIN
   SA_USER_ADMIN.SET_USER_PRIVS(
      policy_name => 'HR_OLS_POLICY',
      user_name   => 'SEC_ADMIN',
      privileges  => 'FULL,PROFILE_ACCESS'
   );
END;
/




-- ============================================================
-- BƯỚC 7: GÁN LABEL CHO TỪNG USER NGHIỆP VỤ
-- Chạy bằng: SEC_ADMIN
--
-- Bảng đối chiếu:
-- ------------------------------------------------------------
-- User     Role RBAC          max_read_label      default_label
-- ------------------------------------------------------------
-- minhlh   hr_manager_role    CONFIDENTIAL:BGD    INTERNAL:HRD
-- maintt   dept_manager_role  CONFIDENTIAL:FIN    INTERNAL:FIN
-- haitv    fin_staff_role     CONFIDENTIAL:BGD    INTERNAL:FIN
-- hungnv   hr_staff_role      CONFIDENTIAL:BGD    INTERNAL:HRD
-- vietpq   dept_member_role   INTERNAL:ITD        INTERNAL:ITD
-- ------------------------------------------------------------
-- Ghi chú:
--  - minhlh/haitv/hungnv dùng group BGD (cha) để nhờ hierarchy
--    đọc được row của mọi phòng con (HRD/FIN/ITD).
--  - vietpq chỉ cấp group ITD, level INTERNAL - đúng nguyên tắc
--    least privilege cho nhân viên thường, KHÔNG có group BGD.
--  - "hungnv không được xem phòng mình" là do VPD chặn (chính
--    sách 1 cho HR), KHÔNG phải OLS - OLS ở đây cố ý permissive.
-- ============================================================

BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'MINHLH',
      max_read_label  => 'CONF::BGD,HRD,FIN,ITD',
      max_write_label => 'CONF::BGD,HRD,FIN,ITD',
      def_label       => 'CONF::BGD,HRD,FIN,ITD'
   );
END;
/

BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'HOATTB',
      max_read_label  => 'CONF::BGD,HRD,FIN,ITD',
      max_write_label => 'CONF::BGD,HRD,FIN,ITD',
      def_label       => 'CONF::BGD,HRD,FIN,ITD'
   );
END;
/
BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'HAITV',
      max_read_label  => 'CONF::BGD,HRD,FIN,ITD',
      max_write_label => 'CONF::BGD,HRD,FIN,ITD',
      def_label       => 'CONF::BGD,HRD,FIN,ITD'
   );
END;
/
BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'HUNGNV',
      max_read_label  => 'CONF::BGD,HRD,FIN,ITD',
      max_write_label => 'CONF::BGD,HRD,FIN,ITD',
      def_label       => 'CONF::BGD,HRD,FIN,ITD'
   );
END;
/
BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'VIETPQ',
      max_read_label  => 'INT::ITD',
      max_write_label => 'INT::ITD',
      def_label       => 'INT::ITD'
   );
END;
/
BEGIN
   SA_USER_ADMIN.SET_USER_LABELS(
      policy_name     => 'HR_OLS_POLICY',
      user_name       => 'MAINTT',
      max_read_label  => 'CONF::FIN',
      max_write_label => 'CONF::FIN',
      def_label       => 'CONF::FIN'
   );
END;
/

-- ============================================================
-- BƯỚC 8: GÁN ROW LABEL CHO DATA HIỆN CÓ
-- Chạy bằng: HR_ADMIN (đã được SET_USER_PRIVS FULL ở Bước 6)
-- ============================================================

-- EMPLOYEES: label = INTERNAL:<mã phòng của chính nhân viên>
UPDATE employees e
SET e.ols_label = CHAR_TO_LABEL(
    'HR_OLS_POLICY',
    'INT::' || (SELECT d.dept_code
                FROM departments d
                WHERE d.department_id = e.department_id)
);

-- PAYROLL: label = CONFIDENTIAL:<mã phòng của nhân viên sở hữu bản lương>
UPDATE payroll p
SET p.ols_label = CHAR_TO_LABEL(
    'HR_OLS_POLICY',
    'CONF::' || (SELECT d.dept_code
                 FROM employees e
                 JOIN departments d ON e.department_id = d.department_id
                 WHERE e.employee_id = p.employee_id)
);


COMMIT;


-- ============================================================
-- BƯỚC 9: TRIGGER TỰ GÁN LABEL KHI INSERT MỚI
-- Chạy bằng: HR_ADMIN
-- ============================================================

CREATE OR REPLACE TRIGGER trg_employees_ols_label
BEFORE INSERT ON employees
FOR EACH ROW
DECLARE
   v_dept_code VARCHAR2(20);
BEGIN
   SELECT dept_code INTO v_dept_code
   FROM departments
   WHERE department_id = :NEW.department_id;

   :NEW.ols_label := CHAR_TO_LABEL('HR_OLS_POLICY', 'INT::' || v_dept_code);
END;
/

CREATE OR REPLACE TRIGGER trg_payroll_ols_label
BEFORE INSERT ON payroll
FOR EACH ROW
DECLARE
   v_dept_code VARCHAR2(20);
BEGIN
   SELECT d.dept_code INTO v_dept_code
   FROM employees e
   JOIN departments d ON e.department_id = d.department_id
   WHERE e.employee_id = :NEW.employee_id;

   :NEW.ols_label := CHAR_TO_LABEL('HR_OLS_POLICY', 'CONF::' || v_dept_code);
END;
/


-- ============================================================
-- BƯỚC 10: VERIFY TRƯỚC KHI ENFORCE
-- Chạy bằng: HR_ADMIN
-- Kiểm tra không còn hàng nào NULL label trước khi bật kiểm soát thật
-- ============================================================

SELECT COUNT(*) AS emp_missing_label
FROM employees
WHERE ols_label IS NULL;

SELECT COUNT(*) AS payroll_missing_label
FROM payroll
WHERE ols_label IS NULL;


-- ============================================================
-- BƯỚC 11: BẬT KIỂM SOÁT THẬT (đổi NO_CONTROL -> READ/WRITE/CHECK)
-- Chạy bằng: SEC_ADMIN
-- Chỉ chạy khi Bước 10 đã trả về 0 cho cả 2 truy vấn
-- ============================================================

BEGIN
   SA_POLICY_ADMIN.REMOVE_TABLE_POLICY(
      policy_name  => 'HR_OLS_POLICY',
      schema_name  => 'HR_ADMIN',
      table_name   => 'EMPLOYEES',
      drop_column  => FALSE
   );
   SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
      policy_name    => 'HR_OLS_POLICY',
      schema_name    => 'HR_ADMIN',
      table_name     => 'EMPLOYEES',
      table_options  => 'READ_CONTROL, HIDE'
   );

   SA_POLICY_ADMIN.REMOVE_TABLE_POLICY(
      policy_name  => 'HR_OLS_POLICY',
      schema_name  => 'HR_ADMIN',
      table_name   => 'PAYROLL',
      drop_column  => FALSE
   );
   SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
      policy_name    => 'HR_OLS_POLICY',
      schema_name    => 'HR_ADMIN',
      table_name     => 'PAYROLL',
      table_options  => 'READ_CONTROL, HIDE'
   );
END;
/


-- ============================================================
-- BƯỚC 12: TEST THEO TỪNG USER (chạy sau khi đã enforce Bước 11)
-- ============================================================

-- vietpq: OLS(INTERNAL:ITD) + VPD(department_id) -> chỉ thấy NV
-- phòng ITD; payroll -> 0 dòng vì level không đủ CONFIDENTIAL
-- (VPD của dept_member vốn cũng đã trả 1=2 -> double-protected)
CONNECT vietpq/vietpq
SELECT employee_id, full_name, department_id FROM employees;
SELECT * FROM payroll;

-- haitv: OLS group BGD (hierarchy) + VPD fin_staff -> xem lương
-- toàn công ty
CONNECT haitv/haitv
SELECT employee_id, base_salary FROM payroll;

-- hungnv: OLS group BGD cho phép đọc mọi group, nhưng VPD
-- employees_vpd_func loại trừ đúng phòng HRD của chính mình
CONNECT hungnv/hungnv
SELECT employee_id, full_name, department_id FROM employees;

-- minhlh: OLS group BGD + level CONFIDENTIAL, VPD IS_HR_MANAGER
-- bỏ qua mọi điều kiện loại trừ -> xem toàn bộ, kể cả phòng mình
CONNECT minhlh/minhlh
SELECT employee_id, full_name, department_id FROM employees;
SELECT employee_id, base_salary FROM payroll;

-- cap quyen WRITE_ACROSS cho minhlh/haitv/hungnv để test insert/update/delete
BEGIN
   SA_USER_ADMIN.SET_USER_PRIVS(
      policy_name => 'HR_OLS_POLICY',
      user_name   => 'MINHLH',
      privileges  => 'WRITEACROSS'
   );
END;
/

BEGIN
   SA_USER_ADMIN.SET_USER_PRIVS(
      policy_name => 'HR_OLS_POLICY',
      user_name   => 'HAITV',
      privileges  => 'WRITEACROSS'
   );
END;
/

BEGIN
   SA_USER_ADMIN.SET_USER_PRIVS(
      policy_name => 'HR_OLS_POLICY',
      user_name   => 'HUNGNV',
      privileges  => 'WRITEACROSS'
   );
END;
/