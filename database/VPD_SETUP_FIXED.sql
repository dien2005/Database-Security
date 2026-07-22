-- ============================================================
-- VIRTUAL PRIVATE DATABASE (VPD) SETUP — ĐÃ SỬA ĐỐI CHIẾU RBAC.sql
-- Mục đích: Thiết lập Application Context, Policy Functions và gắn Policies
-- Chạy bằng: HR_ADMIN hoặc SYS (với các lệnh cần quyền DBA)
-- ============================================================

-- ============================================================
-- BƯỚC 1: TẠO APPLICATION CONTEXT
-- ============================================================

-- Chạy lệnh này bằng SYS AS SYSDBA
create or replace context emp_ctx using hr_admin.vpd_ctx_pkg;

-- Lấy thông tin user đăng nhập và set context (Chạy bằng HR_ADMIN)
create or replace package vpd_ctx_pkg is
   procedure set_context;
end;
/

CREATE OR REPLACE PACKAGE BODY vpd_ctx_pkg IS
   PROCEDURE set_context IS
      v_emp_id  employees.employee_id%TYPE;
      v_dept_id employees.department_id%TYPE;
      v_db_user VARCHAR2(128);
      v_count   NUMBER;
   BEGIN
      v_db_user := SYS_CONTEXT('USERENV','SESSION_USER');
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_INITIALIZING','Y');
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_VALID_EMP','N');

      BEGIN
         SELECT employee_id, department_id
           INTO v_emp_id, v_dept_id
           FROM employees
          WHERE lower(email) = lower(v_db_user) || '@company.com';

         DBMS_SESSION.SET_CONTEXT('EMP_CTX','EMPLOYEE_ID',   TO_CHAR(v_emp_id));
         DBMS_SESSION.SET_CONTEXT('EMP_CTX','DEPARTMENT_ID', TO_CHAR(v_dept_id));
         DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_VALID_EMP',  'Y');
      EXCEPTION
         WHEN NO_DATA_FOUND THEN
            DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_VALID_EMP','N');
      END;

      -- FIX: đọc DBA_ROLE_PRIVS thay vì SESSION_ROLES
      -- Lý do: SESSION_ROLES có race condition với AFTER LOGON trigger
      -- (default role chưa kịp enable khi trigger chạy). DBA_ROLE_PRIVS
      -- phản ánh grant trực tiếp, không phụ thuộc trạng thái session.
      SELECT COUNT(*) INTO v_count
      FROM dba_role_privs
      WHERE grantee = v_db_user AND granted_role = 'HR_MANAGER_ROLE';
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_HR_MANAGER', CASE WHEN v_count > 0 THEN 'Y' ELSE 'N' END);

      SELECT COUNT(*) INTO v_count
      FROM dba_role_privs
      WHERE grantee = v_db_user AND granted_role = 'HR_STAFF_ROLE';
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_HR_STAFF', CASE WHEN v_count > 0 THEN 'Y' ELSE 'N' END);

      SELECT COUNT(*) INTO v_count
      FROM dba_role_privs
      WHERE grantee = v_db_user AND granted_role = 'FIN_STAFF_ROLE';
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_FIN_STAFF', CASE WHEN v_count > 0 THEN 'Y' ELSE 'N' END);

      SELECT COUNT(*) INTO v_count
      FROM dba_role_privs
      WHERE grantee = v_db_user AND granted_role = 'DEPT_MANAGER_ROLE';
      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_DEPT_MANAGER', CASE WHEN v_count > 0 THEN 'Y' ELSE 'N' END);

      DBMS_SESSION.SET_CONTEXT('EMP_CTX','IS_INITIALIZING','N');
   END set_context;
END vpd_ctx_pkg;
/

-- [LƯU Ý]: Chạy lệnh này bằng SYS AS SYSDBA để thiết lập Logon Trigger
create or replace trigger set_emp_ctx_trg
   after logon on database begin
      hr_admin.vpd_ctx_pkg.set_context;
   end;
/


-- ============================================================
-- BƯỚC 2: VIẾT POLICY FUNCTIONS
-- ============================================================

-- ------------------------------------------------------------
-- 2.0 EMPLOYEES — BẢNG MỚI THÊM, trước đây thiếu hoàn toàn
-- Chính sách:
--   - dept_member_role / dept_manager_role : chỉ thấy NV cùng phòng mình
--   - hr_staff_role     : thấy mọi phòng TRỪ phòng của chính mình (chính sách 1 áp dụng cho phòng mình)
--   - hr_manager_role   : thấy + sửa toàn bộ, không trừ phòng nào
--   - fin_staff_role    : kế thừa dept_member -> mặc định chỉ thấy phòng mình ở bảng này
--     (quyền xem employee_id/salary/tax toàn công ty của FIN nằm ở bảng PAYROLL, không phải EMPLOYEES)
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION hr_admin.employees_vpd_func (
   p_schema IN VARCHAR2,
   p_table  IN VARCHAR2
) RETURN VARCHAR2 IS
   v_emp_id    VARCHAR2(10);
   v_dept_id   VARCHAR2(10);
   v_predicate VARCHAR2(1000);
BEGIN
   v_emp_id  := SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID');
   v_dept_id := SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID');

   -- HR_ADMIN/SYS hoặc HR Manager: thấy toàn bộ
   IF SYS_CONTEXT('USERENV','SESSION_USER') IN ('HR_ADMIN','SYS')
      OR SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER') = 'Y' THEN
      RETURN NULL;
   END IF;

   -- Đang trong quá trình logon trigger set context → cho qua
   IF SYS_CONTEXT('EMP_CTX','IS_INITIALIZING') = 'Y'
      OR SYS_CONTEXT('EMP_CTX','IS_INITIALIZING') IS NULL THEN
      RETURN NULL;
   END IF;
   
   -- HR Staff: thấy mọi phòng TRỪ phòng của chính mình
   IF SYS_CONTEXT('EMP_CTX','IS_HR_STAFF') = 'Y' THEN
      RETURN 'department_id != ' || v_dept_id;
   END IF;

   -- Dept Manager / Dept Member / Fin staff: chỉ thấy cùng phòng mình
   IF v_emp_id IS NOT NULL THEN
      v_predicate := 'department_id = ' || v_dept_id;
   ELSE
      v_predicate := '1 = 2';
   END IF;

   RETURN v_predicate;
END employees_vpd_func;
/


-- ------------------------------------------------------------
-- 2.1 LEAVE_REQUESTS
-- Mọi role chỉ xem/sửa đơn nghỉ phép của chính mình, trừ HR_ADMIN/SYS
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION hr_admin.leave_requests_vpd_func (
   p_schema IN VARCHAR2,
   p_table  IN VARCHAR2
) RETURN VARCHAR2 IS
   v_emp_id    VARCHAR2(10);
   v_dept_id   VARCHAR2(10);
   v_is_valid  VARCHAR2(1);
   v_predicate VARCHAR2(400);
BEGIN
   v_emp_id   := SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID');
   v_is_valid := SYS_CONTEXT('EMP_CTX','IS_VALID_EMP');
   v_dept_id  := SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID');

   IF SYS_CONTEXT('USERENV','SESSION_USER') IN ('HR_ADMIN','SYS')
      OR SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER') = 'Y' THEN
      RETURN NULL;
   END IF;

   -- HR Staff: thấy đơn của phòng khác + đơn của chính mình
   IF SYS_CONTEXT('EMP_CTX','IS_HR_STAFF') = 'Y' THEN
      RETURN 'employee_id = ' || v_emp_id
          || ' OR employee_id IN (SELECT employee_id FROM hr_admin.employees'
          || ' WHERE department_id != ' || v_dept_id || ')';
   END IF;

   -- Mọi role khác: chỉ thấy đơn của chính mình
   IF v_is_valid = 'Y' AND v_emp_id IS NOT NULL THEN
      v_predicate := 'employee_id = ' || v_emp_id;
   ELSE
      v_predicate := '1 = 2';
   END IF;

   RETURN v_predicate;
END leave_requests_vpd_func;
/


-- ------------------------------------------------------------
-- 2.2 PAYROLL
-- Chính sách:
--   - dept_manager_role : xem lương CẢ PHÒNG mình
--   - hr_staff_role     : xem lương MỌI nhân viên TRỪ phòng HR của chính mình
--   - hr_manager_role   : xem lương toàn công ty
--   - fin_staff_role    : xem lương toàn công ty (để tính lương)
--   - dept_member_role  : KHÔNG xem lương ai cả, kể cả của chính mình
--     (theo đúng câu chữ đề bài "ngoại trừ thông tin về lương" — XÁC NHẬN LẠI nếu muốn cho tự xem lương mình)
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION hr_admin.payroll_vpd_func (
   p_schema IN VARCHAR2,
   p_table  IN VARCHAR2
) RETURN VARCHAR2 IS
   v_emp_id    VARCHAR2(10);
   v_dept_id   VARCHAR2(10);
   v_predicate VARCHAR2(1000);
BEGIN
   v_emp_id  := SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID');
   v_dept_id := SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID');

   -- HR_ADMIN/SYS, HR Manager, hoặc Fin staff: thấy toàn bộ lương công ty
   IF SYS_CONTEXT('USERENV','SESSION_USER') IN ('HR_ADMIN','SYS')
      OR SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER') = 'Y'
      OR SYS_CONTEXT('EMP_CTX','IS_FIN_STAFF')  = 'Y' THEN
      RETURN NULL;
   END IF;

   -- HR Staff: lương của MỌI nhân viên TRỪ phòng HR của chính mình
   IF SYS_CONTEXT('EMP_CTX','IS_HR_STAFF') = 'Y' THEN
      v_predicate := 'employee_id IN (SELECT employee_id FROM hr_admin.employees WHERE department_id != ' || v_dept_id || ')';
      RETURN v_predicate;
   END IF;

   -- Dept Manager: lương của CẢ PHÒNG mình
   IF SYS_CONTEXT('EMP_CTX','IS_DEPT_MANAGER') = 'Y' THEN
      v_predicate := 'employee_id IN (SELECT employee_id FROM hr_admin.employees WHERE department_id = ' || v_dept_id || ')';
      RETURN v_predicate;
   END IF;

   -- Dept Member thường: KHÔNG xem lương ai, kể cả chính mình
   RETURN '1 = 2';
END payroll_vpd_func;
/


-- ------------------------------------------------------------
-- 2.3 CERTIFICATES
-- Chính sách: áp dụng tương tự EMPLOYEES (đi kèm thông tin nhân viên)
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION hr_admin.certificates_vpd_func (
   p_schema IN VARCHAR2,
   p_table  IN VARCHAR2
) RETURN VARCHAR2 IS
   v_emp_id    VARCHAR2(10);
   v_dept_id   VARCHAR2(10);
   v_predicate VARCHAR2(1000);
BEGIN
   v_emp_id  := SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID');
   v_dept_id := SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID');

   IF SYS_CONTEXT('USERENV','SESSION_USER') IN ('HR_ADMIN','SYS')
      OR SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER') = 'Y' THEN
      RETURN NULL;
   END IF;

   IF SYS_CONTEXT('EMP_CTX','IS_HR_STAFF') = 'Y' THEN
      v_predicate := 'employee_id IN (SELECT employee_id FROM hr_admin.employees WHERE department_id != ' || v_dept_id || ')';
      RETURN v_predicate;
   END IF;

   IF v_emp_id IS NOT NULL THEN
      v_predicate := 'employee_id = ' || v_emp_id;
   ELSE
      v_predicate := '1 = 2';
   END IF;

   RETURN v_predicate;
END certificates_vpd_func;
/


-- ------------------------------------------------------------
-- 2.4 PROJECTS 
-- Chính sách: HR (staff+manager) thấy toàn bộ dự án (để quản lý nhân sự dự án);
-- các role khác chỉ thấy dự án thuộc phòng mình hoặc dự án được phân công
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION hr_admin.projects_vpd_func (
   p_schema IN VARCHAR2,
   p_table  IN VARCHAR2
) RETURN VARCHAR2 IS
   v_emp_id    VARCHAR2(10);
   v_dept_id   VARCHAR2(10);
   v_predicate VARCHAR2(1000);
BEGIN
   v_emp_id  := SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID');
   v_dept_id := SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID');

   IF SYS_CONTEXT('USERENV','SESSION_USER') IN ('HR_ADMIN','SYS')
      OR SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER') = 'Y'
      OR SYS_CONTEXT('EMP_CTX','IS_HR_STAFF')   = 'Y' THEN
      RETURN NULL;
   END IF;

   IF v_emp_id IS NOT NULL THEN
      v_predicate := 'department_id = ' || v_dept_id
                  || ' OR project_id IN (SELECT project_id FROM hr_admin.assignments WHERE employee_id = ' || v_emp_id || ')';
   ELSE
      v_predicate := '1 = 2';
   END IF;

   RETURN v_predicate;
END projects_vpd_func;
/

-- ============================================================
-- BƯỚC 3: ATTACH POLICY 
-- Chạy bằng SYS
-- ============================================================

BEGIN
    -- Xóa policy cũ nếu đã tồn tại (tránh lỗi ORA-28101 khi chạy lại script)
    BEGIN DBMS_RLS.DROP_POLICY('HR_ADMIN', 'EMPLOYEES', 'employees_policy'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_RLS.DROP_POLICY('HR_ADMIN', 'LEAVE_REQUESTS', 'leave_requests_policy'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_RLS.DROP_POLICY('HR_ADMIN', 'PAYROLL', 'payroll_policy'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_RLS.DROP_POLICY('HR_ADMIN', 'CERTIFICATES', 'certificates_policy'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_RLS.DROP_POLICY('HR_ADMIN', 'PROJECTS', 'projects_policy'); EXCEPTION WHEN OTHERS THEN NULL; END;

    -- Gắn policy vào bảng EMPLOYEES (MỚI)
    DBMS_RLS.ADD_POLICY (
        object_schema    => 'HR_ADMIN',
        object_name      => 'EMPLOYEES',
        policy_name      => 'employees_policy',
        function_schema  => 'HR_ADMIN',
        policy_function  => 'employees_vpd_func',
        statement_types  => 'SELECT, INSERT, UPDATE',
        update_check     => TRUE -- Ngăn HR_STAFF sửa employee sang department_id thuộc phòng mình
    );

    -- Gắn policy vào bảng LEAVE_REQUESTS
    DBMS_RLS.ADD_POLICY (
        object_schema    => 'HR_ADMIN',
        object_name      => 'LEAVE_REQUESTS',
        policy_name      => 'leave_requests_policy',
        function_schema  => 'HR_ADMIN',
        policy_function  => 'leave_requests_vpd_func',
        statement_types  => 'SELECT, UPDATE, DELETE',
        update_check     => TRUE
    );

    -- Gắn policy vào bảng PAYROLL
    DBMS_RLS.ADD_POLICY (
        object_schema    => 'HR_ADMIN',
        object_name      => 'PAYROLL',
        policy_name      => 'payroll_policy',
        function_schema  => 'HR_ADMIN',
        policy_function  => 'payroll_vpd_func',
        statement_types  => 'SELECT, UPDATE, INSERT',
        update_check     => TRUE
    );

    -- Gắn policy vào bảng CERTIFICATES
    DBMS_RLS.ADD_POLICY (
        object_schema    => 'HR_ADMIN',
        object_name      => 'CERTIFICATES',
        policy_name      => 'certificates_policy',
        function_schema  => 'HR_ADMIN',
        policy_function  => 'certificates_vpd_func',
        statement_types  => 'SELECT, UPDATE, DELETE',
        update_check     => TRUE
    );

    -- Gắn policy vào bảng PROJECTS
    DBMS_RLS.ADD_POLICY (
        object_schema    => 'HR_ADMIN',
        object_name      => 'PROJECTS',
        policy_name      => 'projects_policy',
        function_schema  => 'HR_ADMIN',
        policy_function  => 'projects_vpd_func',
        statement_types  => 'SELECT'
    );
END;
/



