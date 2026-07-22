-- ============================================================
-- PHẦN B — Chạy bằng: SYS AS SYSDBA
-- Tạo DB user cho hungnv (test hr_staff_role trực tiếp)
-- lanth KHÔNG cần tạo user vì chỉ dùng làm "đồng nghiệp" để test
-- việc vietpq nhìn thấy đúng người cùng phòng
-- ============================================================

BEGIN
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HUNGNV') LOOP
        EXECUTE IMMEDIATE 'DROP USER hungnv CASCADE';
    END LOOP;
END;
/

CREATE USER hungnv IDENTIFIED BY hungnv
    DEFAULT TABLESPACE users
    QUOTA UNLIMITED ON users;

GRANT CREATE SESSION TO hungnv;
GRANT hr_staff_role TO hungnv;

CREATE OR REPLACE SYNONYM hungnv.employees        FOR hr_admin.employees;
CREATE OR REPLACE SYNONYM hungnv.departments      FOR hr_admin.departments;
CREATE OR REPLACE SYNONYM hungnv.jobs             FOR hr_admin.jobs;
CREATE OR REPLACE SYNONYM hungnv.locations        FOR hr_admin.locations;
CREATE OR REPLACE SYNONYM hungnv.payroll          FOR hr_admin.payroll;
CREATE OR REPLACE SYNONYM hungnv.leave_requests   FOR hr_admin.leave_requests;
CREATE OR REPLACE SYNONYM hungnv.certificates     FOR hr_admin.certificates;
CREATE OR REPLACE SYNONYM hungnv.assignments      FOR hr_admin.assignments;
CREATE OR REPLACE SYNONYM hungnv.projects         FOR hr_admin.projects;

COMMIT;


-- ============================================================
-- VERIFY DATA SAU KHI CHẠY XONG (chạy bằng HR_ADMIN)
-- ============================================================

SELECT e.full_name, e.email, d.dept_code, e.status
FROM employees e JOIN departments d ON e.department_id = d.department_id
ORDER BY d.dept_code, e.full_name;

SELECT p.employee_id, e.email, d.dept_code, p.pay_period, p.base_salary, p.status
FROM payroll p
JOIN employees e ON p.employee_id = e.employee_id
JOIN departments d ON e.department_id = d.department_id
ORDER BY d.dept_code, p.pay_period;