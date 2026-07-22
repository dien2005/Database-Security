-- ============================================================
--                          RBAC
-- ============================================================
-- ============================================================
-- ROLE HIERARCHY (role bên dưới được role bên trên kế thừa toàn bộ quyền)
--
--   dept_member_role
--     ├── dept_manager_role
--     ├── hr_staff_role
--     │     └── hr_manager_role
--     └── fin_staff_role



-- ============================================================
-- GIAI ĐOẠN 2: RBAC (Role-Based Access Control)
-- Chạy bằng: SYS AS SYSDBA
-- Chạy SAU: 00_SYS_SETUP.sql và 01_HR_ADMIN_DDL_INSERT.sql
-- ============================================================
-- Mapping chính sách đề bài → Roles:
--
--  DEPT_MEMBER_ROLE     → Nhân viên thường: xem đồng nghiệp cùng phòng (trừ lương)
--  DEPT_MANAGER_ROLE    → Trưởng phòng thường: xem đầy đủ kể cả lương, không sửa
--  HR_STAFF_ROLE        → Nhân viên HR: xem + sửa mọi phòng khác, trừ phòng mình
--  HR_MANAGER_ROLE      → Trưởng phòng HR: toàn quyền xem + sửa + giám sát
--  FIN_STAFF_ROLE       → Nhân viên Kế toán: xem employee_id, salary toàn công ty
-- ============================================================



-- ============================================================
-- BƯỚC 1: XOÁ ROLES 
-- ============================================================

BEGIN
    FOR r IN (
        SELECT role FROM dba_roles
        WHERE role IN (
            'DEPT_MEMBER_ROLE',
            'DEPT_MANAGER_ROLE',
            'HR_STAFF_ROLE',
            'HR_MANAGER_ROLE',
            'FIN_STAFF_ROLE'
        )
    ) LOOP
        EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
    END LOOP;
END;
/



-- ============================================================
-- BƯỚC 2: TẠO ROLES
-- ============================================================

-- Role cho nhân viên thường (mọi phòng ban)
-- Quyền: SELECT một số cột của EMPLOYEES (không có quyền xem lương))
CREATE ROLE dept_member_role;

-- Role cho trưởng phòng thường (không phải HR)
-- Quyền: SELECT đầy đủ EMPLOYEES + PAYROLL của nhân viên trong phòng
CREATE ROLE dept_manager_role;

-- Role cho nhân viên phòng HR
-- Quyền: SELECT + EMPLOYEES INFORMATION (trừ phòng của mình - VPD xử lý)
CREATE ROLE hr_staff_role;

-- Role cho trưởng phòng HR (HR Manager)
-- Quyền: toàn quyền + giám sát audit log
CREATE ROLE hr_manager_role;

-- Role cho nhân viên phòng Kế toán
-- Quyền: SELECT employee_id, base_salary của mọi nhân viên (cho tính lương)
CREATE ROLE fin_staff_role;



-- ============================================================
-- BƯỚC 3: CẤP PRIVILEGES CHO TỪNG ROLE
-- ============================================================

-- -----------------------------------------------------------
-- 3A. DEPT_MEMBER_ROLE
--     Nhân viên thường: xem đồng nghiệp cùng phòng, KHÔNG có lương
--     VPD sẽ lọc theo department_id → role chỉ cần SELECT
-- -----------------------------------------------------------

-- Xem thông tin nhân viên (VPD sẽ lọc theo phòng ban)
GRANT SELECT ON hr_admin.employees       TO dept_member_role;

-- Xem thông tin phòng ban, chức danh, địa điểm (metadata công khai)
GRANT SELECT ON hr_admin.departments     TO dept_member_role;
GRANT SELECT ON hr_admin.jobs            TO dept_member_role;
GRANT SELECT ON hr_admin.locations       TO dept_member_role;

-- Xem chứng chỉ (VPD lọc theo phòng)
GRANT SELECT ON hr_admin.certificates    TO dept_member_role;

-- Xem đơn nghỉ phép của chính mình (VPD lọc theo employee_id)
GRANT SELECT ON hr_admin.leave_requests  TO dept_member_role;

-- Xem assignments (dự án - VPD loc assignments)
GRANT SELECT ON hr_admin.assignments     TO dept_member_role;
GRANT SELECT ON hr_admin.projects        TO dept_member_role;



-- -----------------------------------------------------------
-- 3B. DEPT_MANAGER_ROLE
--     Trưởng phòng thường: kế thừa member + xem lương trong phòng
--     VPD lọc department_id
-- -----------------------------------------------------------

-- Kế thừa toàn bộ quyền của member
GRANT dept_member_role TO dept_manager_role;

-- Thêm quyền xem PAYROLL (VPD sẽ lọc phòng ban)
GRANT SELECT ON hr_admin.payroll         TO dept_manager_role;

-- Xem pending approvals liên quan đến phòng mình (read-only)
GRANT SELECT ON hr_admin.pending_approvals TO dept_manager_role;
-- Chạy bằng SYS AS SYSDBA, thêm vào RBAC.sql sau phần 3B
GRANT UPDATE ON hr_admin.pending_approvals TO dept_manager_role;

-- -----------------------------------------------------------
-- 3C. HR_STAFF_ROLE
--     Nhân viên HR: xem + sửa mọi nhân viên NGOẠI TRỪ phòng mình
--     (chính sách 1 vẫn áp dụng cho phòng mình → VPD xử lý)
--     Lưu ý: HR được xem lương — nhưng CHỈ của phòng khác 
--     → VPD Giai đoạn 3 PHẢI lọc: hr_staff_role xem được bản ghi của
--       department_id != phòng mình;
--     Vẫn giữ chính sách 1: payroll cùng phòng mình thì KHÔNG xem được.
-- -----------------------------------------------------------

-- Kế thừa member (để áp chính sách 1 cho phòng mình)
GRANT dept_member_role TO hr_staff_role;

-- Xem và sửa thông tin nhân viên của PHÒNG KHÁC (VPD xử lý)
GRANT SELECT ON hr_admin.employees       TO hr_staff_role;
GRANT INSERT ON hr_admin.employees       TO hr_staff_role;
GRANT UPDATE ON hr_admin.employees       TO hr_staff_role;
--GRANT DELETE ON hr_admin.employees       TO hr_staff_role; (HR staff KHÔNG được xoá nhân viên nghỉ việc)

-- Xem và sửa đơn nghỉ phép (phòng khác)
GRANT SELECT ON hr_admin.leave_requests  TO hr_staff_role;
GRANT INSERT ON hr_admin.leave_requests  TO hr_staff_role;
GRANT UPDATE ON hr_admin.leave_requests  TO hr_staff_role;

-- Xem và sửa chứng chỉ (phòng khác)
GRANT SELECT ON hr_admin.certificates    TO hr_staff_role;
GRANT INSERT ON hr_admin.certificates    TO hr_staff_role;
GRANT UPDATE ON hr_admin.certificates    TO hr_staff_role;

-- Xem assignments và projects (phòng khác)
GRANT SELECT ON hr_admin.assignments     TO hr_staff_role;
GRANT SELECT ON hr_admin.projects        TO hr_staff_role;

-- HR_STAFF_ROLE: thêm quyền xem payroll
GRANT SELECT ON hr_admin.payroll TO hr_staff_role;

-- HR staff KHÔNG được xem audit log
-- Cấp quyền tạo yêu cầu phê duyệt cho Nhân viên Nhân sự
GRANT INSERT ON hr_admin.pending_approvals TO hr_staff_role;

-- -----------------------------------------------------------
-- 3D. HR_MANAGER_ROLE
--     Trưởng phòng HR: toàn quyền + giám sát
--     QUAN TRỌNG cho VPD (GĐ3): khác với hr_staff_role, hr_manager_role
--     KHÔNG bị trừ "phòng mình" — được xem + sửa employees/payroll của
--     MỌI phòng kể cả phòng HR. Policy function phải check role này
--     riêng để bỏ qua điều kiện loại trừ department_id.
-- -----------------------------------------------------------

-- Kế thừa HR staff
GRANT hr_staff_role TO hr_manager_role;

-- Thêm quyền xem PAYROLL của mọi nhân viên
--GRANT SELECT ON hr_admin.payroll           TO hr_manager_role;

-- Quyền xem + sửa thông tin nhân viên HR của chính phòng mình
-- (Override VPD restriction của hr_staff_role - sẽ xử lý ở giai đoạn VPD)
--GRANT UPDATE ON hr_admin.employees         TO hr_manager_role;

-- Xem và quản lý pending approvals
GRANT SELECT ON hr_admin.pending_approvals TO hr_manager_role;
GRANT INSERT ON hr_admin.pending_approvals TO hr_manager_role;
GRANT UPDATE ON hr_admin.pending_approvals TO hr_manager_role;

-- Xem audit log (thông qua synonym)
-- Quyền SELECT trên system_audit_log đã cấp cho bgd_user ở SYS_SETUP
-- HR Manager cần được cấp thêm:
GRANT SELECT ON audit_owner.system_audit_log TO hr_manager_role;

-- Quản lý phân loại dữ liệu và chính sách (đọc)
GRANT SELECT ON hr_admin.data_classification TO hr_manager_role;
GRANT SELECT ON hr_admin.redaction_policies   TO hr_manager_role;

-- Quản lý approval matrix (đọc)
GRANT SELECT ON hr_admin.approval_matrix      TO hr_manager_role;

-- hr_manager duyệt (đổi status -> APPROVED)
GRANT UPDATE ON hr_admin.payroll TO hr_manager_role;
-- hr_manager them phieu luong (INSERT) khi payroll status = PENDING
GRANT INSERT ON hr_admin.payroll TO hr_manager_role;
-- -----------------------------------------------------------
-- 3E. FIN_STAFF_ROLE
--     Nhân viên Kế toán: chính sách 1 + xem employee_id/salary toàn công ty
-- -----------------------------------------------------------

-- Kế thừa member (chính sách 1 cho phòng mình)
GRANT dept_member_role TO fin_staff_role;

-- Xem thông tin tính lương: employee_id + salary + 
-- (chỉ 3 cột cụ thể, không phải toàn bộ EMPLOYEES)
GRANT SELECT ON hr_admin.employees TO fin_staff_role;
-- Lưu ý: VPD/Redaction sẽ giới hạn cột hiển thị với fin_staff


-- Xem PAYROLL để tính lương (VPD không lọc phòng ban cho fin_staff)
GRANT SELECT ON hr_admin.payroll TO fin_staff_role;
-- fin_staff tạo phiếu lương + sửa khi còn PENDING
GRANT INSERT ON hr_admin.payroll TO fin_staff_role;
GRANT UPDATE ON hr_admin.payroll TO fin_staff_role;


-- Xem danh sách phòng ban (để join)
GRANT SELECT ON hr_admin.departments TO fin_staff_role;
-- cấp quyền xem danh sách nhân viên để lookup employee_id khi tạo phiếu lương
GRANT EXECUTE ON hr_admin.fn_get_payroll_employee_lookup TO fin_staff_role;

-- Cấp quyền tương tự cho Kế toán (nếu kế toán cũng cần tạo phê duyệt)
GRANT INSERT ON hr_admin.pending_approvals TO fin_staff_role;

COMMIT;

-- ============================================================
-- BƯỚC 4: GÁN ROLES CHO ORACLE DB USERS
-- (Users đã được tạo ở User_Data.sql và SYS_SETUP.sql)
-- ============================================================

-- Lê Hoàng Minh - Giám đốc Nhân sự (HR Manager)
-- email: minhlh@company.com
GRANT hr_manager_role TO minhlh;

-- Nguyễn Thị Mai - Trưởng phòng Kế toán (Department Manager)
-- email: maintt@company.com
GRANT dept_manager_role TO maintt;

-- Trần Văn Hải - Chuyên viên Kế toán (Finance Staff)
-- email: haitv@company.com
GRANT fin_staff_role TO haitv;

-- Phạm Quốc Việt - Lập trình viên (Department Member)
-- email: vietpq@company.com
GRANT dept_member_role TO vietpq;

-- BGD_USER đã có SELECT trên audit_owner.system_audit_log (từ SYS_SETUP)
-- Không cần thêm role nghiệp vụ



-- ============================================================
-- BƯỚC 5: TẠO SYNONYMS CHO USERS (để gọi bảng không cần prefix)
-- ============================================================

-- minhlh (HR Manager)
CREATE OR REPLACE SYNONYM minhlh.employees          FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM minhlh.departments         FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM minhlh.jobs                FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM minhlh.locations           FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM minhlh.payroll             FOR hr_admin.payroll;
CREATE OR REPLACE SYNONYM minhlh.leave_requests      FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM minhlh.certificates        FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM minhlh.assignments         FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM minhlh.projects            FOR hr_admin.projects;
CREATE OR REPLACE SYNONYM minhlh.pending_approvals   FOR hr_admin.pending_approvals;
CREATE OR REPLACE SYNONYM minhlh.data_classification FOR hr_admin.data_classification;
CREATE OR REPLACE SYNONYM minhlh.system_audit_log    FOR audit_owner.system_audit_log;

-- maintt (Trưởng phòng Kế toán)
CREATE OR REPLACE SYNONYM maintt.employees        FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM maintt.departments      FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM maintt.jobs             FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM maintt.locations        FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM maintt.payroll          FOR hr_admin.payroll;
CREATE OR REPLACE SYNONYM maintt.leave_requests   FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM maintt.certificates     FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM maintt.assignments      FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM maintt.projects         FOR hr_admin.projects;
CREATE OR REPLACE SYNONYM maintt.pending_approvals FOR hr_admin.pending_approvals;

-- haitv (Chuyên viên Kế toán)
CREATE OR REPLACE SYNONYM haitv.employees      FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM haitv.departments    FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM haitv.jobs           FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM haitv.locations      FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM haitv.payroll        FOR hr_admin.payroll;
CREATE OR REPLACE SYNONYM haitv.leave_requests FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM haitv.certificates   FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM haitv.assignments    FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM haitv.projects       FOR hr_admin.projects;
CREATE OR REPLACE SYNONYM haitv.pending_approvals FOR hr_admin.pending_approvals;

-- vietpq (Lập trình viên)
CREATE OR REPLACE SYNONYM vietpq.employees      FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM vietpq.departments    FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM vietpq.jobs           FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM vietpq.locations      FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM vietpq.leave_requests FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM vietpq.certificates   FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM vietpq.assignments    FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM vietpq.projects       FOR hr_admin.projects;

-- hoattb (Phó phòng Nhân sự)
BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HOATTB') LOOP
        EXECUTE IMMEDIATE 'DROP USER hoattb CASCADE';
    END LOOP;
END;
/

CREATE USER hoattb IDENTIFIED BY hoattb
    DEFAULT TABLESPACE users
    QUOTA 0 ON users;

GRANT CREATE SESSION TO hoattb;
GRANT hr_manager_role TO hoattb;

CREATE OR REPLACE SYNONYM hoattb.employees          FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM hoattb.departments         FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM hoattb.jobs                FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM hoattb.locations           FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM hoattb.payroll             FOR hr_admin.payroll;
CREATE OR REPLACE SYNONYM hoattb.leave_requests      FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM hoattb.certificates        FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM hoattb.assignments         FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM hoattb.projects            FOR hr_admin.projects;
CREATE OR REPLACE SYNONYM hoattb.pending_approvals   FOR hr_admin.pending_approvals;

COMMIT;


-- xu ly cho fin_staff_role (nhan vien ke toan) - xem ten toan bo nhan vien
CREATE OR REPLACE FUNCTION hr_admin.fn_get_payroll_employee_lookup
RETURN SYS_REFCURSOR
AUTHID DEFINER
AS
    v_cursor SYS_REFCURSOR;
BEGIN
    OPEN v_cursor FOR
        SELECT e.employee_id,
               e.full_name || ' (' || d.dept_code || ')' AS display_name
        FROM employees e
        JOIN departments d ON e.department_id = d.department_id
        WHERE e.status = 'ACTIVE'
        ORDER BY e.full_name;
    RETURN v_cursor;
END;
/