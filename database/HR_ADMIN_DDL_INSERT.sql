-- Chạy bằng: HR_ADMIN
-- Mục đích: Tạo 14 bảng + insert data mẫu
-- Chạy SAU file 00_SYS_SETUP.sql
-- Lưu ý: SYSTEM_AUDIT_LOG đã được SYS tạo trong schema AUDIT_OWNER
--        HR_ADMIN chỉ có synonym trỏ đến — không phải owner



-- BƯỚC 0: DROP TABLES NẾU TỒN TẠI
-- Chạy ngược FK dependency
-- Lưu ý: SYSTEM_AUDIT_LOG không nằm đây vì thuộc AUDIT_OWNER
BEGIN
    FOR t IN (
        SELECT table_name FROM user_tables
        WHERE table_name IN (
            'REDACTION_POLICIES','DATA_CLASSIFICATION',
            'PENDING_APPROVALS','APPROVAL_MATRIX',
            'SESSION_CONTEXT_LOG',
            'CERTIFICATES','LEAVE_REQUESTS','PAYROLL',
            'ASSIGNMENTS','PROJECTS',
            'EMPLOYEES','DEPARTMENTS','JOBS','LOCATIONS'
        )
    ) LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP TABLE ' || t.table_name
                              || ' CASCADE CONSTRAINTS PURGE';
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Khong the drop ' || t.table_name
                                     || ': ' || SQLERRM);
        END;
    END LOOP;
END;
/


-- NHÓM 1: BẢNG KHÔNG CÓ FK NGOÀI
-- Thứ tự: LOCATIONS → JOBS → APPROVAL_MATRIX


-- TABLE 1: LOCATIONS

CREATE TABLE locations (
    location_id     NUMBER          GENERATED ALWAYS AS IDENTITY,
    location_name   VARCHAR2(100)   NOT NULL,
    address         VARCHAR2(255),
    city            VARCHAR2(100),
    country         VARCHAR2(100),
    timezone        VARCHAR2(50),
    status          VARCHAR2(20)    DEFAULT 'ACTIVE',

    CONSTRAINT pk_locations
        PRIMARY KEY (location_id),
    CONSTRAINT chk_loc_status
        CHECK (status IN ('ACTIVE','INACTIVE'))
);

COMMENT ON TABLE  locations             IS 'Thông tin chi nhánh và khu vực địa lý';
COMMENT ON COLUMN locations.location_id IS 'Khóa chính tự sinh';
COMMENT ON COLUMN locations.timezone    IS 'Múi giờ chuẩn TZ, VD: Asia/Ho_Chi_Minh';
COMMENT ON COLUMN locations.status      IS 'ACTIVE | INACTIVE';



-- TABLE 2: JOBS

CREATE TABLE jobs (
    job_id          NUMBER          GENERATED ALWAYS AS IDENTITY,
    job_title       VARCHAR2(100)   NOT NULL,
    job_code        VARCHAR2(20)    NOT NULL,
    min_salary      NUMBER(12,2),
    max_salary      NUMBER(12,2),
    job_level       VARCHAR2(20),
    status          VARCHAR2(20)    DEFAULT 'ACTIVE',

    CONSTRAINT pk_jobs
        PRIMARY KEY (job_id),
    CONSTRAINT uq_job_code
        UNIQUE (job_code),
    CONSTRAINT chk_job_level
        CHECK (job_level IN ('JUNIOR','MID','SENIOR','LEAD','MANAGER','DIRECTOR')),
    CONSTRAINT chk_job_salary
        CHECK (
            min_salary IS NULL
            OR max_salary IS NULL
            OR min_salary <= max_salary
        ),
    CONSTRAINT chk_job_status
        CHECK (status IN ('ACTIVE','INACTIVE'))
);

COMMENT ON TABLE  jobs           IS 'Danh mục chức danh và vị trí công việc';
COMMENT ON COLUMN jobs.job_code  IS 'Mã chức danh duy nhất dùng cho tích hợp hệ thống ngoài';
COMMENT ON COLUMN jobs.job_level IS 'JUNIOR | MID | SENIOR | LEAD | MANAGER | DIRECTOR';



-- TABLE 3: APPROVAL_MATRIX

CREATE TABLE approval_matrix (
    matrix_id           NUMBER          GENERATED ALWAYS AS IDENTITY,
    action_type         VARCHAR2(50)    NOT NULL,
    description         VARCHAR2(255),
    min_approvers       NUMBER          DEFAULT 2,
    approver_role_1     VARCHAR2(50)    NOT NULL,
    approver_role_2     VARCHAR2(50),
    require_diff_dept   CHAR(1)         DEFAULT 'Y',
    is_active           CHAR(1)         DEFAULT 'Y',

    CONSTRAINT pk_approval_matrix
        PRIMARY KEY (matrix_id),
    CONSTRAINT uq_approval_action
        UNIQUE (action_type),
    CONSTRAINT chk_approval_min_approvers
        CHECK (min_approvers BETWEEN 1 AND 5),
    CONSTRAINT chk_approval_diff_dept
        CHECK (require_diff_dept IN ('Y','N')),
    CONSTRAINT chk_approval_is_active
        CHECK (is_active IN ('Y','N'))
);

COMMENT ON TABLE  approval_matrix                   IS 'Cấu hình 4-eyes principle cho từng loại hành động nhạy cảm';
COMMENT ON COLUMN approval_matrix.action_type       IS 'VD: SALARY_CHANGE, ROLE_ASSIGN, DATA_EXPORT, PAYROLL_APPROVE';
COMMENT ON COLUMN approval_matrix.min_approvers     IS 'Số người tối thiểu phải approve';
COMMENT ON COLUMN approval_matrix.approver_role_1   IS 'DB role bắt buộc của approver thứ nhất';
COMMENT ON COLUMN approval_matrix.approver_role_2   IS 'DB role bắt buộc của approver thứ hai (nếu cần)';
COMMENT ON COLUMN approval_matrix.require_diff_dept IS 'Y = hai approver phải thuộc phòng ban khác nhau';




-- NHÓM 2: VÒNG FK — DEPARTMENTS <-> EMPLOYEES
-- Strategy: tạo DEPARTMENTS không có fk_dept_manager,
--             tạo EMPLOYEES đầy đủ FK,
--             ALTER TABLE thêm fk_dept_manager ở cuối



-- TABLE 4: DEPARTMENTS (chưa có fk_dept_manager)

CREATE TABLE departments (
    department_id   NUMBER          GENERATED ALWAYS AS IDENTITY,
    dept_name       VARCHAR2(100)   NOT NULL,
    dept_code       VARCHAR2(20)    NOT NULL,
    manager_id      NUMBER,
    location_id     NUMBER,
    parent_dept_id  NUMBER,
    status          VARCHAR2(20)    DEFAULT 'ACTIVE',
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,

    CONSTRAINT pk_departments
        PRIMARY KEY (department_id),
    CONSTRAINT uq_dept_code
        UNIQUE (dept_code),
    CONSTRAINT chk_dept_status
        CHECK (status IN ('ACTIVE','INACTIVE','DISSOLVED')),
    CONSTRAINT fk_dept_location
        FOREIGN KEY (location_id)
        REFERENCES locations(location_id),
    CONSTRAINT fk_dept_parent
        FOREIGN KEY (parent_dept_id)
        REFERENCES departments(department_id)
    -- fk_dept_manager → ALTER TABLE sau
);

COMMENT ON TABLE  departments                IS 'Thông tin phòng ban trong tổ chức';
COMMENT ON COLUMN departments.dept_code      IS 'Mã phòng ban duy nhất dùng cho tích hợp hệ thống';
COMMENT ON COLUMN departments.manager_id     IS 'FK DEFERRABLE đến EMPLOYEES - thêm bằng ALTER sau';
COMMENT ON COLUMN departments.parent_dept_id IS 'Self-reference: phòng ban cha, hỗ trợ cấu trúc phân cấp';
COMMENT ON COLUMN departments.status         IS 'ACTIVE | INACTIVE | DISSOLVED';



-- TABLE 5: EMPLOYEES

CREATE TABLE employees (
    employee_id     NUMBER          GENERATED ALWAYS AS IDENTITY,
    full_name       VARCHAR2(100)   NOT NULL,
    email           VARCHAR2(150)   NOT NULL,
    phone           VARCHAR2(20),
    hire_date       DATE            NOT NULL,
    job_id          NUMBER          NOT NULL,
    department_id   NUMBER          NOT NULL,
    manager_id      NUMBER,
    location_id     NUMBER          NOT NULL,
    status          VARCHAR2(20)    DEFAULT 'ACTIVE',
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,
    created_by      VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),

    CONSTRAINT pk_employees
        PRIMARY KEY (employee_id),
    CONSTRAINT uq_emp_email
        UNIQUE (email),
    CONSTRAINT chk_emp_status
        CHECK (status IN ('ACTIVE','INACTIVE','TERMINATED')),
    CONSTRAINT fk_emp_job
        FOREIGN KEY (job_id)
        REFERENCES jobs(job_id),
    CONSTRAINT fk_emp_dept
        FOREIGN KEY (department_id)
        REFERENCES departments(department_id),
    CONSTRAINT fk_emp_manager
        FOREIGN KEY (manager_id)
        REFERENCES employees(employee_id),
    CONSTRAINT fk_emp_location
        FOREIGN KEY (location_id)
        REFERENCES locations(location_id)
);

COMMENT ON TABLE  employees             IS 'Bảng nhân sự chính - anchor cho VPD và OLS';
COMMENT ON COLUMN employees.employee_id IS 'Khóa chính tự sinh, không thể insert giá trị tùy ý';
COMMENT ON COLUMN employees.email       IS 'Định danh thứ cấp dùng cho authentication tầng application';
COMMENT ON COLUMN employees.manager_id  IS 'Self-reference: quản lý trực tiếp, dùng xây cây phân cấp VPD';
COMMENT ON COLUMN employees.status      IS 'ACTIVE | INACTIVE | TERMINATED (không DELETE nhân viên nghỉ việc)';
COMMENT ON COLUMN employees.created_by  IS 'DB user thực sự tạo record - lấy từ session context, không giả mạo được';



-- NHÓM 3: BẢNG NGHIỆP VỤ
-- Thứ tự: PROJECTS → ASSIGNMENTS → PAYROLL
--         → LEAVE_REQUESTS → CERTIFICATES


-- TABLE 6: PROJECTS

CREATE TABLE projects (
    project_id      NUMBER          GENERATED ALWAYS AS IDENTITY,
    project_name    VARCHAR2(200)   NOT NULL,
    project_code    VARCHAR2(30)    NOT NULL,
    department_id   NUMBER,
    start_date      DATE,
    end_date        DATE,
    budget          NUMBER(15,2),
    status          VARCHAR2(20)    DEFAULT 'PLANNING',
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,
    created_by      VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),

    CONSTRAINT pk_projects
        PRIMARY KEY (project_id),
    CONSTRAINT uq_project_code
        UNIQUE (project_code),
    CONSTRAINT chk_project_status
        CHECK (status IN ('PLANNING','ACTIVE','ON_HOLD','COMPLETED','CANCELLED')),
    CONSTRAINT chk_project_dates
        CHECK (start_date IS NULL OR end_date IS NULL OR start_date <= end_date),
    CONSTRAINT chk_project_budget
        CHECK (budget IS NULL OR budget >= 0),
    CONSTRAINT fk_project_dept
        FOREIGN KEY (department_id)
        REFERENCES departments(department_id)
);

COMMENT ON TABLE  projects              IS 'Danh sách dự án đang triển khai';
COMMENT ON COLUMN projects.project_code IS 'Mã dự án duy nhất dùng cho báo cáo và tích hợp';
COMMENT ON COLUMN projects.budget       IS 'Ngân sách dự án - cần FGA khi SELECT/UPDATE';


-- TABLE 7: ASSIGNMENTS

CREATE TABLE assignments (
    assignment_id   NUMBER          GENERATED ALWAYS AS IDENTITY,
    employee_id     NUMBER          NOT NULL,
    project_id      NUMBER          NOT NULL,
    role_in_project VARCHAR2(100),
    assigned_date   DATE            DEFAULT SYSDATE,
    end_date        DATE,
    allocation_pct  NUMBER(5,2),
    assigned_by     VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),
    status          VARCHAR2(20)    DEFAULT 'ACTIVE',

    CONSTRAINT pk_assignments
        PRIMARY KEY (assignment_id),
    CONSTRAINT uq_assignment
        UNIQUE (employee_id, project_id, assigned_date),
    CONSTRAINT chk_assign_allocation
        CHECK (allocation_pct IS NULL OR allocation_pct BETWEEN 0 AND 100),
    CONSTRAINT chk_assign_dates
        CHECK (assigned_date IS NULL OR end_date IS NULL OR assigned_date <= end_date),
    CONSTRAINT chk_assign_status
        CHECK (status IN ('ACTIVE','COMPLETED','CANCELLED')),
    CONSTRAINT fk_assign_employee
        FOREIGN KEY (employee_id)
        REFERENCES employees(employee_id),
    CONSTRAINT fk_assign_project
        FOREIGN KEY (project_id)
        REFERENCES projects(project_id)
);

COMMENT ON TABLE  assignments                IS 'Phân công nhân viên vào dự án';
COMMENT ON COLUMN assignments.allocation_pct IS 'Phần trăm thời gian phân bổ cho dự án (0-100)';
COMMENT ON COLUMN assignments.assigned_by    IS 'DB user thực hiện phân công - lấy từ session context';
COMMENT ON COLUMN assignments.role_in_project IS 'Vai trò: Tech Lead, Developer, BA, QA...';



-- TABLE 8: PAYROLL
-- Nhạy cảm nhất: OLS CONFIDENTIAL + VPD + Data Redaction

CREATE TABLE payroll (
    payroll_id      NUMBER          GENERATED ALWAYS AS IDENTITY,
    employee_id     NUMBER          NOT NULL,
    pay_period      VARCHAR2(7)     NOT NULL,
    base_salary     NUMBER(12,2)    NOT NULL,
    bonus           NUMBER(12,2)    DEFAULT 0,
    allowances      NUMBER(12,2)    DEFAULT 0,
    deductions      NUMBER(12,2)    DEFAULT 0,
    net_pay         NUMBER(12,2)    GENERATED ALWAYS AS
                        (base_salary + bonus + allowances - deductions) VIRTUAL,
    pay_date        DATE,
    approved_by     VARCHAR2(128),
    approval_time   TIMESTAMP,
    status          VARCHAR2(20)    DEFAULT 'PENDING',
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,
    created_by      VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),

    CONSTRAINT pk_payroll
        PRIMARY KEY (payroll_id),
    CONSTRAINT uq_payroll_period
        UNIQUE (employee_id, pay_period),
    CONSTRAINT chk_payroll_period_fmt
        CHECK (REGEXP_LIKE(pay_period, '^\d{4}-(0[1-9]|1[0-2])$')),
    CONSTRAINT chk_payroll_base_salary
        CHECK (base_salary > 0),
    CONSTRAINT chk_payroll_bonus
        CHECK (bonus >= 0),
    CONSTRAINT chk_payroll_allowances
        CHECK (allowances >= 0),
    CONSTRAINT chk_payroll_deductions
        CHECK (deductions >= 0),
    CONSTRAINT chk_payroll_status
        CHECK (status IN ('PENDING','APPROVED','PAID','CANCELLED')),
    CONSTRAINT fk_payroll_employee
        FOREIGN KEY (employee_id)
        REFERENCES employees(employee_id)
);

COMMENT ON TABLE  payroll             IS 'Chi tiết lương - OLS: CONFIDENTIAL, cần Data Redaction';
COMMENT ON COLUMN payroll.pay_period  IS 'Kỳ lương định dạng YYYY-MM, VD: 2026-05';
COMMENT ON COLUMN payroll.net_pay     IS 'Virtual column: base_salary + bonus + allowances - deductions';
COMMENT ON COLUMN payroll.status      IS 'PENDING | APPROVED | PAID | CANCELLED';
COMMENT ON COLUMN payroll.approved_by IS 'DB user phê duyệt - phải khác người tạo (SoD)';


-- TABLE 9: LEAVE_REQUESTS

CREATE TABLE leave_requests (
    leave_id        NUMBER          GENERATED ALWAYS AS IDENTITY,
    employee_id     NUMBER          NOT NULL,
    leave_type      VARCHAR2(50)    NOT NULL,
    start_date      DATE            NOT NULL,
    end_date        DATE            NOT NULL,
    total_days      NUMBER(5,1)     GENERATED ALWAYS AS
                        (end_date - start_date + 1) VIRTUAL,
    reason          VARCHAR2(500),
    status          VARCHAR2(20)    DEFAULT 'PENDING',
    reviewed_by     VARCHAR2(128),
    reviewed_at     TIMESTAMP,
    reject_reason   VARCHAR2(500),
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,
    created_by      VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),

    CONSTRAINT pk_leave_requests
        PRIMARY KEY (leave_id),
    CONSTRAINT chk_leave_type
        CHECK (leave_type IN ('ANNUAL','SICK','MATERNITY','PATERNITY','UNPAID','OTHER')),
    CONSTRAINT chk_leave_dates
        CHECK (start_date <= end_date),
    CONSTRAINT chk_leave_status
        CHECK (status IN ('PENDING','APPROVED','REJECTED','CANCELLED')),
    CONSTRAINT fk_leave_employee
        FOREIGN KEY (employee_id)
        REFERENCES employees(employee_id)
);

COMMENT ON TABLE  leave_requests               IS 'Quản lý nghỉ phép - VPD: nhân viên chỉ thấy của mình';
COMMENT ON COLUMN leave_requests.total_days    IS 'Virtual column: end_date - start_date + 1';
COMMENT ON COLUMN leave_requests.reject_reason IS 'Lý do từ chối - bắt buộc khi status = REJECTED';
COMMENT ON COLUMN leave_requests.leave_type    IS 'ANNUAL | SICK | MATERNITY | PATERNITY | UNPAID | OTHER';


-- TABLE 10: CERTIFICATES

CREATE TABLE certificates (
    cert_id         NUMBER          GENERATED ALWAYS AS IDENTITY,
    employee_id     NUMBER          NOT NULL,
    cert_name       VARCHAR2(200)   NOT NULL,
    issuing_body    VARCHAR2(200),
    cert_number     VARCHAR2(100),
    issue_date      DATE,
    expiry_date     DATE,
    is_verified     CHAR(1)         DEFAULT 'N',
    verified_by     VARCHAR2(128),
    verified_at     TIMESTAMP,
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,

    CONSTRAINT pk_certificates
        PRIMARY KEY (cert_id),
    CONSTRAINT chk_cert_verified
        CHECK (is_verified IN ('Y','N')),
    CONSTRAINT chk_cert_dates
        CHECK (
            issue_date IS NULL
            OR expiry_date IS NULL
            OR issue_date <= expiry_date
        ),
    CONSTRAINT fk_cert_employee
        FOREIGN KEY (employee_id)
        REFERENCES employees(employee_id)
);

COMMENT ON TABLE  certificates             IS 'Bằng cấp và chứng chỉ của nhân viên';
COMMENT ON COLUMN certificates.is_verified IS 'N = chưa xác minh, Y = HR đã xác minh thực tế';
COMMENT ON COLUMN certificates.verified_by IS 'Chỉ HR_ADMIN được UPDATE cột này';
COMMENT ON COLUMN certificates.cert_number IS 'Số serial chứng chỉ (nullable)';



-- NHÓM 4: BẢNG HỖ TRỢ BẢO MẬT
-- Lưu ý: SYSTEM_AUDIT_LOG đã được SYS tạo trong AUDIT_OWNER
--        và có synonym trỏ về — không tạo lại




-- TABLE 11: SESSION_CONTEXT_LOG

CREATE TABLE session_context_log (
    session_log_id      NUMBER          GENERATED ALWAYS AS IDENTITY,
    session_id          VARCHAR2(64)    NOT NULL,
    db_user             VARCHAR2(128)   NOT NULL,
    app_user            VARCHAR2(128),
    login_time          TIMESTAMP       DEFAULT SYSTIMESTAMP,
    logout_time         TIMESTAMP,
    ip_address          VARCHAR2(45),
    machine_name        VARCHAR2(128),
    program_name        VARCHAR2(255),
    context_hash        VARCHAR2(128),
    is_valid            CHAR(1)         DEFAULT 'Y',
    invalidated_at      TIMESTAMP,
    invalidated_reason  VARCHAR2(255),

    CONSTRAINT pk_session_context
        PRIMARY KEY (session_log_id),
    CONSTRAINT uq_session_login
        UNIQUE (session_id, login_time),
    CONSTRAINT chk_session_is_valid
        CHECK (is_valid IN ('Y','N')),
    CONSTRAINT chk_session_times
        CHECK (logout_time IS NULL OR login_time <= logout_time)
);

COMMENT ON TABLE  session_context_log                   IS 'Track và validate session - VPD dùng detect session hijacking';
COMMENT ON COLUMN session_context_log.context_hash      IS 'Hash IP+machine+program+db_user, detect anomaly giữa chừng';
COMMENT ON COLUMN session_context_log.is_valid          IS 'N = session bị flag, VPD trả WHERE 1=0';
COMMENT ON COLUMN session_context_log.invalidated_reason IS 'IP_CHANGE | MACHINE_CHANGE | FORCED_LOGOUT';



-- TABLE 12: PENDING_APPROVALS

CREATE TABLE pending_approvals (
    approval_id         NUMBER          GENERATED ALWAYS AS IDENTITY,
    action_type         VARCHAR2(50)    NOT NULL,
    target_table        VARCHAR2(128),
    target_id           NUMBER,
    requested_by        VARCHAR2(128)   NOT NULL,
    request_time        TIMESTAMP       DEFAULT SYSTIMESTAMP,
    payload             CLOB,
    approved_by_1       VARCHAR2(128),
    approval_time_1     TIMESTAMP,
    approved_by_2       VARCHAR2(128),
    approval_time_2     TIMESTAMP,
    status              VARCHAR2(20)    DEFAULT 'PENDING',
    final_action_time   TIMESTAMP,
    notes               VARCHAR2(500),

    CONSTRAINT pk_pending_approvals
        PRIMARY KEY (approval_id),
    CONSTRAINT chk_approval_status
        CHECK (status IN ('PENDING','APPROVED','REJECTED')),
    CONSTRAINT chk_no_self_approve_1
        CHECK (requested_by != approved_by_1),
    CONSTRAINT chk_no_self_approve_2
        CHECK (requested_by != approved_by_2),
    CONSTRAINT chk_no_same_approver
        CHECK (approved_by_1 IS NULL OR approved_by_2 IS NULL
               OR approved_by_1 != approved_by_2),
    CONSTRAINT fk_approval_action_type
        FOREIGN KEY (action_type)
        REFERENCES approval_matrix(action_type)
);

COMMENT ON TABLE  pending_approvals              IS '4-eyes principle: thay đổi nhạy cảm cần ít nhất 2 người approve';
COMMENT ON COLUMN pending_approvals.payload      IS 'JSON snapshot thay đổi đề xuất - không apply ngay, lưu chờ approve';
COMMENT ON COLUMN pending_approvals.requested_by IS 'Không được trùng approved_by_1 hoặc approved_by_2';
COMMENT ON COLUMN pending_approvals.status       IS 'PENDING | APPROVED | REJECTED - không bao giờ DELETE';



-- TABLE 13: DATA_CLASSIFICATION

CREATE TABLE data_classification (
    class_id        NUMBER          GENERATED ALWAYS AS IDENTITY,
    table_name      VARCHAR2(128)   NOT NULL,
    column_name     VARCHAR2(128),
    ols_label       VARCHAR2(30)    NOT NULL,
    gdpr_category   VARCHAR2(50)    DEFAULT 'NONE',
    redaction_type  VARCHAR2(30)    DEFAULT 'NONE',
    retention_days  NUMBER,
    last_reviewed   DATE,
    reviewed_by     VARCHAR2(128),
    notes           VARCHAR2(500),

    CONSTRAINT pk_data_classification
        PRIMARY KEY (class_id),
    CONSTRAINT uq_data_class
        UNIQUE (table_name, column_name),
    CONSTRAINT chk_ols_label
        CHECK (ols_label IN ('PUBLIC','INTERNAL','CONFIDENTIAL','SECRET')),
    CONSTRAINT chk_gdpr_category
        CHECK (gdpr_category IN ('NONE','PERSONAL','SENSITIVE','FINANCIAL')),
    CONSTRAINT chk_redaction_type
        CHECK (redaction_type IN ('NONE','FULL','PARTIAL','REGEXP')),
    CONSTRAINT chk_retention_days
        CHECK (retention_days IS NULL OR retention_days > 0)
);

COMMENT ON TABLE  data_classification             IS 'Catalog: OLS label + GDPR mapping + Redaction policy cho từng bảng/cột';
COMMENT ON COLUMN data_classification.column_name IS 'NULL = label cả bảng; column-level override table-level';
COMMENT ON COLUMN data_classification.ols_label   IS 'PUBLIC | INTERNAL | CONFIDENTIAL | SECRET';
COMMENT ON COLUMN data_classification.retention_days IS 'Số ngày lưu trước khi archive/purge';



-- TABLE 14: REDACTION_POLICIES

CREATE TABLE redaction_policies (
    policy_id       NUMBER          GENERATED ALWAYS AS IDENTITY,
    policy_name     VARCHAR2(128)   NOT NULL,
    object_schema   VARCHAR2(128)   NOT NULL,
    object_name     VARCHAR2(128)   NOT NULL,
    column_name     VARCHAR2(128)   NOT NULL,
    redaction_type  VARCHAR2(30)    NOT NULL,
    expression      CLOB,
    is_active       CHAR(1)         DEFAULT 'Y',
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP,
    created_by      VARCHAR2(128)   DEFAULT SYS_CONTEXT('USERENV','SESSION_USER'),
    last_modified   TIMESTAMP,
    modified_by     VARCHAR2(128),
    notes           VARCHAR2(500),

    CONSTRAINT pk_redaction_policies
        PRIMARY KEY (policy_id),
    CONSTRAINT uq_redaction_policy_name
        UNIQUE (policy_name),
    CONSTRAINT chk_redaction_type_pol
        CHECK (redaction_type IN ('FULL','PARTIAL','REGEXP','RANDOM')),
    CONSTRAINT chk_redaction_is_active
        CHECK (is_active IN ('Y','N'))
);

COMMENT ON TABLE  redaction_policies           IS 'Track DBMS_REDACT policy đang chạy - ai tạo, khi nào sửa';
COMMENT ON COLUMN redaction_policies.expression IS 'Điều kiện trigger redaction';
COMMENT ON COLUMN redaction_policies.is_active  IS 'N = policy đã drop nhưng giữ lịch sử';
COMMENT ON COLUMN redaction_policies.policy_name IS 'Phải match chính xác tên trong DBMS_REDACT.ADD_POLICY';



-- BƯỚC CUỐI DDL: ALTER TABLE thêm FK vòng tròn
-- DEPARTMENTS.manager_id → EMPLOYEES.employee_id

ALTER TABLE departments
    ADD CONSTRAINT fk_dept_manager
        FOREIGN KEY (manager_id)
        REFERENCES employees(employee_id)
        DEFERRABLE INITIALLY DEFERRED;



-- INSERT DATA MẪU
-- Thứ tự theo FK dependency



-- 1. LOCATIONS

INSERT INTO locations (location_name, address, city, country, timezone, status)
VALUES ('Chi nhanh Mien Nam', '123 Nguyen Hue', 'Ho Chi Minh', 'Viet Nam', 'Asia/Ho_Chi_Minh', 'ACTIVE');

INSERT INTO locations (location_name, address, city, country, timezone, status)
VALUES ('Chi nhanh Mien Trung', '456 Tran Phu', 'Da Nang', 'Viet Nam', 'Asia/Ho_Chi_Minh', 'ACTIVE');



-- 2. JOBS

INSERT INTO jobs (job_title, job_code, min_salary, max_salary, job_level, status)
VALUES ('Giam Doc Nhan Su', 'CHRO', 40000000, 80000000, 'DIRECTOR', 'ACTIVE');

INSERT INTO jobs (job_title, job_code, min_salary, max_salary, job_level, status)
VALUES ('Truong Phong Ke Toan', 'FIN_LEAD', 25000000, 50000000, 'MANAGER', 'ACTIVE');

INSERT INTO jobs (job_title, job_code, min_salary, max_salary, job_level, status)
VALUES ('Chuyen Vien Ke Toan', 'FIN_EMP', 12000000, 22000000, 'MID', 'ACTIVE');

INSERT INTO jobs (job_title, job_code, min_salary, max_salary, job_level, status)
VALUES ('Lap Trinh Vien', 'DEV', 15000000, 35000000, 'MID', 'ACTIVE');



-- 3. DEPARTMENTS (manager_id = NULL trước, UPDATE sau)

INSERT INTO departments (dept_name, dept_code, manager_id, location_id, parent_dept_id, status)
VALUES ('Ban Giam Doc', 'BGD', NULL, 1, NULL, 'ACTIVE');

INSERT INTO departments (dept_name, dept_code, manager_id, location_id, parent_dept_id, status)
VALUES ('Phong Nhan Su', 'HRD', NULL, 1, 1, 'ACTIVE');

INSERT INTO departments (dept_name, dept_code, manager_id, location_id, parent_dept_id, status)
VALUES ('Phong Ke Toan', 'FIN', NULL, 1, 1, 'ACTIVE');

INSERT INTO departments (dept_name, dept_code, manager_id, location_id, parent_dept_id, status)
VALUES ('Phong IT', 'ITD', NULL, 2, 1, 'ACTIVE');



-- 4. EMPLOYEES
-- Dùng subquery lấy job_id và department_id theo code
-- tránh hardcode ID phụ thuộc sequence


-- Nhân viên 1: Giám đốc Nhân sự - quản lý toàn bộ HR
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Le Hoang Minh',
    'minhlh@company.com',
    '0901234567',
    TO_DATE('2020-01-15', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'CHRO'),
    (SELECT department_id FROM departments WHERE dept_code = 'HRD'),
    NULL,
    (SELECT location_id FROM locations WHERE city = 'Ho Chi Minh'),
    'ACTIVE'
);

-- Nhân viên 2: Trưởng phòng Kế toán - approver lương chặng 1
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Nguyen Thi Mai',
    'maintt@company.com',
    '0912345678',
    TO_DATE('2021-03-10', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'FIN_LEAD'),
    (SELECT department_id FROM departments WHERE dept_code = 'FIN'),
    NULL,
    (SELECT location_id FROM locations WHERE city = 'Ho Chi Minh'),
    'ACTIVE'
);

-- Nhân viên 3: Chuyên viên Kế toán - người lập bảng lương (test SoD)
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Tran Van Hai',
    'haitv@company.com',
    '0923456789',
    TO_DATE('2023-06-01', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'FIN_EMP'),
    (SELECT department_id FROM departments WHERE dept_code = 'FIN'),
    (SELECT employee_id FROM employees WHERE email = 'maintt@company.com'),
    (SELECT location_id FROM locations WHERE city = 'Ho Chi Minh'),
    'ACTIVE'
);

-- Nhân viên 4: Lập trình viên - test xem lương của chính mình
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Pham Quoc Viet',
    'vietpq@company.com',
    '0934567890',
    TO_DATE('2024-02-20', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'DEV'),
    (SELECT department_id FROM departments WHERE dept_code = 'ITD'),
    NULL,
    (SELECT location_id FROM locations WHERE city = 'Da Nang'),
    'ACTIVE'
);



-- 5. UPDATE DEPARTMENTS: gán trưởng phòng sau khi có EMPLOYEES
-- fk_dept_manager là DEFERRABLE → check lúc COMMIT

UPDATE departments
    SET manager_id = (SELECT employee_id FROM employees WHERE email = 'minhlh@company.com')
WHERE dept_code = 'HRD';

UPDATE departments
    SET manager_id = (SELECT employee_id FROM employees WHERE email = 'maintt@company.com')
WHERE dept_code = 'FIN';



-- 6. PROJECTS

INSERT INTO projects (project_name, project_code, department_id, start_date, budget, status)
VALUES (
    'He thong ERP noi bo',
    'ERP_2026',
    (SELECT department_id FROM departments WHERE dept_code = 'ITD'),
    TO_DATE('2026-01-01', 'YYYY-MM-DD'),
    500000000,
    'ACTIVE'
);



-- 7. ASSIGNMENTS

INSERT INTO assignments (employee_id, project_id, role_in_project, allocation_pct, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'vietpq@company.com'),
    (SELECT project_id FROM projects WHERE project_code = 'ERP_2026'),
    'Developer',
    100.00,
    'ACTIVE'
);



-- 8. PAYROLL


-- Lương tháng 05/2026 của Lập trình viên - đã APPROVED
INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances,
                     deductions, approved_by, approval_time, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'vietpq@company.com'),
    '2026-05',
    20000000, 2000000, 1000000, 500000,
    'maintt@company.com',
    SYSTIMESTAMP,
    'APPROVED'
);

-- Lương tháng 06/2026 của Chuyên viên Kế toán - đang PENDING
INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances, deductions, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'haitv@company.com'),
    '2026-06',
    15000000, 0, 1000000, 0,
    'PENDING'
);



-- 9. LEAVE_REQUESTS

INSERT INTO leave_requests (employee_id, leave_type, start_date, end_date, reason, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'vietpq@company.com'),
    'ANNUAL',
    TO_DATE('2026-07-01', 'YYYY-MM-DD'),
    TO_DATE('2026-07-03', 'YYYY-MM-DD'),
    'Di choi cung gia dinh',
    'PENDING'
);



-- 10. CERTIFICATES

INSERT INTO certificates (employee_id, cert_name, issuing_body, cert_number, issue_date, is_verified)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'vietpq@company.com'),
    'Oracle Certified Professional Java SE 17',
    'Oracle',
    'OCP17-99238',
    TO_DATE('2025-12-10', 'YYYY-MM-DD'),
    'N'
);



-- 11. APPROVAL_MATRIX

INSERT INTO approval_matrix (action_type, description, min_approvers,
                              approver_role_1, approver_role_2,
                              require_diff_dept, is_active)
VALUES ('SALARY_CHANGE',
        'Duyet thay doi luong nhan vien',
        2, 'FIN_ADMIN_ROLE', 'HR_ADMIN_ROLE', 'Y', 'Y');

INSERT INTO approval_matrix (action_type, description, min_approvers,
                              approver_role_1, approver_role_2,
                              require_diff_dept, is_active)
VALUES ('ROLE_ASSIGN',
        'Duyet phan quyen role moi cho user',
        2, 'HR_ADMIN_ROLE', 'DBA_ROLE', 'Y', 'Y');

INSERT INTO approval_matrix (action_type, description, min_approvers,
                              approver_role_1, approver_role_2,
                              require_diff_dept, is_active)
VALUES ('DATA_EXPORT',
        'Duyet xuat du lieu ngoai he thong',
        2, 'HR_ADMIN_ROLE', 'FIN_ADMIN_ROLE', 'Y', 'Y');



-- 12. PENDING_APPROVALS
-- Tran Van Hai yêu cầu tăng lương cho Pham Quoc Viet

INSERT INTO pending_approvals (action_type, target_table, target_id,
                                requested_by, payload, status)
VALUES (
    'SALARY_CHANGE',
    'PAYROLL',
    (SELECT payroll_id FROM payroll
     WHERE employee_id = (SELECT employee_id FROM employees
                          WHERE email = 'vietpq@company.com')
     AND pay_period = '2026-05'),
    'haitv@company.com',
    '{"employee_id":"vietpq","old_salary":20000000,"new_salary":25000000,"note":"Tang luong vi hoan thanh tot du an ERP"}',
    'PENDING'
);



-- 13. DATA_CLASSIFICATION

-- Cấp bảng
INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, notes)
VALUES ('PAYROLL', NULL, 'CONFIDENTIAL', 'FINANCIAL', 'NONE', 'Toan bo bang PAYROLL: CONFIDENTIAL');

INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, notes)
VALUES ('EMPLOYEES', NULL, 'INTERNAL', 'PERSONAL', 'NONE', 'Toan bo bang EMPLOYEES: INTERNAL');

-- Cấp cột - override table-level
INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, retention_days, notes)
VALUES ('PAYROLL', 'BASE_SALARY', 'CONFIDENTIAL', 'FINANCIAL', 'FULL', 2555, 'Che toan bo voi user khong co quyen PAYROLL');

INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, retention_days, notes)
VALUES ('PAYROLL', 'BONUS', 'SECRET', 'FINANCIAL', 'FULL', 2555, 'Che toan bo - chi PAYROLL_ADMIN va C-level thay');

INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, retention_days, notes)
VALUES ('EMPLOYEES', 'PHONE', 'INTERNAL', 'PERSONAL', 'PARTIAL', 1825, 'Che mot phan so dien thoai');

INSERT INTO data_classification (table_name, column_name, ols_label, gdpr_category, redaction_type, notes)
VALUES ('SYSTEM_AUDIT_LOG', NULL, 'SECRET', 'NONE', 'NONE', 'Chi SYS va BGD_USER moi xem duoc');



-- 14. REDACTION_POLICIES (catalog, DBMS_REDACT chạy bằng SYS sau)

INSERT INTO redaction_policies (policy_name, object_schema, object_name,
                                 column_name, redaction_type, expression, notes)
VALUES (
    'REDACT_SALARY_FULL',
    'HR_ADMIN', 'PAYROLL', 'BASE_SALARY',
    'FULL',
    'SYS_CONTEXT(''SYS_SESSION_ROLES'',''PAYROLL_ADMIN_ROLE'') IS NULL',
    'Che luong co ban voi moi user khong co role PAYROLL_ADMIN_ROLE'
);

INSERT INTO redaction_policies (policy_name, object_schema, object_name,
                                 column_name, redaction_type, expression, notes)
VALUES (
    'REDACT_BONUS_FULL',
    'HR_ADMIN', 'PAYROLL', 'BONUS',
    'FULL',
    'SYS_CONTEXT(''SYS_SESSION_ROLES'',''PAYROLL_ADMIN_ROLE'') IS NULL',
    'Che thuong voi moi user khong co role PAYROLL_ADMIN_ROLE'
);

INSERT INTO redaction_policies (policy_name, object_schema, object_name,
                                 column_name, redaction_type, expression, notes)
VALUES (
    'REDACT_PHONE_PARTIAL',
    'HR_ADMIN', 'EMPLOYEES', 'PHONE',
    'PARTIAL',
    'SYS_CONTEXT(''SYS_SESSION_ROLES'',''HR_ADMIN_ROLE'') IS NULL',
    'Che 5 so cuoi SDT voi user khong co role HR_ADMIN_ROLE'
);



-- VERIFY SAU KHI CHẠY


-- Kiểm tra 14 bảng (13 trong HR_ADMIN + 1 Blockchain trong AUDIT_OWNER)
SELECT
    'HR_ADMIN'      AS schema_name,
    table_name,
    'STANDARD'      AS table_type
FROM user_tables
WHERE table_name IN (
    'LOCATIONS','JOBS','APPROVAL_MATRIX','DEPARTMENTS','EMPLOYEES',
    'PROJECTS','ASSIGNMENTS','PAYROLL','LEAVE_REQUESTS','CERTIFICATES',
    'SESSION_CONTEXT_LOG','PENDING_APPROVALS',
    'DATA_CLASSIFICATION','REDACTION_POLICIES'
)
UNION ALL
SELECT
    'AUDIT_OWNER'   AS schema_name,
    'SYSTEM_AUDIT_LOG',
    'BLOCKCHAIN'    AS table_type
FROM dual
ORDER BY schema_name, table_name;


-- Kiểm tra số lượng record mỗi bảng
SELECT 'LOCATIONS'          AS tbl, COUNT(*) AS cnt FROM locations          UNION ALL
SELECT 'JOBS'               AS tbl, COUNT(*) AS cnt FROM jobs               UNION ALL
SELECT 'DEPARTMENTS'        AS tbl, COUNT(*) AS cnt FROM departments         UNION ALL
SELECT 'EMPLOYEES'          AS tbl, COUNT(*) AS cnt FROM employees           UNION ALL
SELECT 'PROJECTS'           AS tbl, COUNT(*) AS cnt FROM projects            UNION ALL
SELECT 'ASSIGNMENTS'        AS tbl, COUNT(*) AS cnt FROM assignments         UNION ALL
SELECT 'PAYROLL'            AS tbl, COUNT(*) AS cnt FROM payroll             UNION ALL
SELECT 'LEAVE_REQUESTS'     AS tbl, COUNT(*) AS cnt FROM leave_requests      UNION ALL
SELECT 'CERTIFICATES'       AS tbl, COUNT(*) AS cnt FROM certificates        UNION ALL
SELECT 'APPROVAL_MATRIX'    AS tbl, COUNT(*) AS cnt FROM approval_matrix     UNION ALL
SELECT 'PENDING_APPROVALS'  AS tbl, COUNT(*) AS cnt FROM pending_approvals   UNION ALL
SELECT 'DATA_CLASSIFICATION'AS tbl, COUNT(*) AS cnt FROM data_classification UNION ALL
SELECT 'REDACTION_POLICIES' AS tbl, COUNT(*) AS cnt FROM redaction_policies
ORDER BY tbl;


-- Kiểm tra constraints
SELECT
    table_name,
    constraint_name,
    constraint_type,
    status,
    deferrable,
    deferred
FROM user_constraints
WHERE table_name IN (
    'LOCATIONS','JOBS','APPROVAL_MATRIX','DEPARTMENTS','EMPLOYEES',
    'PROJECTS','ASSIGNMENTS','PAYROLL','LEAVE_REQUESTS','CERTIFICATES',
    'SESSION_CONTEXT_LOG','PENDING_APPROVALS',
    'DATA_CLASSIFICATION','REDACTION_POLICIES'
)
AND constraint_type IN ('P','U','R','C')
ORDER BY table_name, constraint_type, constraint_name;

COMMIT;




-- ============================================================
-- BỔ SUNG DATA ĐỂ TEST VPD/RBAC ĐẦY ĐỦ
-- Chạy SAU: HR_ADMIN_DDL_INSERT.sql, RBAC.sql, VPD_SETUP_FIXED.sql
-- Gồm 2 phần: PHẦN A chạy bằng HR_ADMIN, PHẦN B chạy bằng SYS
-- ============================================================


-- ============================================================
-- PHẦN A — Chạy bằng: HR_ADMIN
-- ============================================================

-- A1. Thêm job cho nhân viên HR thường (chưa có job_code này)
INSERT INTO jobs (job_title, job_code, min_salary, max_salary, job_level, status)
VALUES ('Chuyen Vien Nhan Su', 'HR_EMP', 12000000, 20000000, 'MID', 'ACTIVE');


-- A2. Nhân viên HR thường (KHÔNG phải manager) — để test hr_staff_role
--     Cùng phòng HRD với minhlh (hr_manager)
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Nguyen Van Hung',
    'hungnv@company.com',
    '0945678901',
    TO_DATE('2023-09-01', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'HR_EMP'),
    (SELECT department_id FROM departments WHERE dept_code = 'HRD'),
    (SELECT employee_id FROM employees WHERE email = 'minhlh@company.com'),
    (SELECT location_id FROM locations WHERE city = 'Ho Chi Minh'),
    'ACTIVE'
);


-- A3. Nhân viên IT thứ 2 — đồng nghiệp cùng phòng với vietpq
--     Để test dept_member_role chỉ thấy CÙNG PHÒNG, không thấy phòng khác
INSERT INTO employees (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
VALUES (
    'Ngo Thi Lan',
    'lanth@company.com',
    '0956789012',
    TO_DATE('2024-05-10', 'YYYY-MM-DD'),
    (SELECT job_id FROM jobs WHERE job_code = 'DEV'),
    (SELECT department_id FROM departments WHERE dept_code = 'ITD'),
    NULL,
    (SELECT location_id FROM locations WHERE city = 'Da Nang'),
    'ACTIVE'
);


-- A4. PAYROLL cho các nhân viên mới — để test filter theo phòng ban
INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances, deductions, approved_by, approval_time, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'hungnv@company.com'),
    '2026-06', 16000000, 1000000, 500000, 200000,
    NULL, NULL, 'PENDING'
);

INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances, deductions, approved_by, approval_time, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'lanth@company.com'),
    '2026-06', 21000000, 0, 500000, 0,
    NULL, NULL, 'PENDING'
);

-- Lương của chính minhlh (HR Manager) — để test hr_staff KHÔNG được thấy
-- (vì cùng phòng HRD với hungnv), còn hr_manager/fin_staff thì thấy
INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances, deductions, approved_by, approval_time, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'minhlh@company.com'),
    '2026-06', 65000000, 8000000, 3000000, 1500000,
    'haitv@company.com', SYSTIMESTAMP, 'APPROVED'
);

-- Lương tháng 06 của maintt (Trưởng phòng FIN) — để test dept_manager xem lương cả phòng
INSERT INTO payroll (employee_id, pay_period, base_salary, bonus, allowances, deductions, approved_by, approval_time, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'maintt@company.com'),
    '2026-06', 38000000, 3000000, 1000000, 500000,
    'hr_admin', SYSTIMESTAMP, 'APPROVED'
);


-- A5. LEAVE_REQUESTS cho các nhân viên mới
INSERT INTO leave_requests (employee_id, leave_type, start_date, end_date, reason, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'hungnv@company.com'),
    'SICK',
    TO_DATE('2026-07-05', 'YYYY-MM-DD'),
    TO_DATE('2026-07-06', 'YYYY-MM-DD'),
    'Bi cam',
    'PENDING'
);

INSERT INTO leave_requests (employee_id, leave_type, start_date, end_date, reason, status)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'lanth@company.com'),
    'ANNUAL',
    TO_DATE('2026-07-10', 'YYYY-MM-DD'),
    TO_DATE('2026-07-12', 'YYYY-MM-DD'),
    'Nghi le',
    'PENDING'
);

INSERT INTO leave_requests (employee_id, leave_type, start_date, end_date, reason, status, reviewed_by, reviewed_at)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'maintt@company.com'),
    'ANNUAL',
    TO_DATE('2026-06-20', 'YYYY-MM-DD'),
    TO_DATE('2026-06-22', 'YYYY-MM-DD'),
    'Viec gia dinh',
    'APPROVED',
    'minhlh@company.com',
    SYSTIMESTAMP
);


-- A6. CERTIFICATES cho các nhân viên mới
INSERT INTO certificates (employee_id, cert_name, issuing_body, cert_number, issue_date, is_verified, verified_by, verified_at)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'hungnv@company.com'),
    'Chung chi Nhan su chuyen nghiep',
    'PACE',
    'HR-2023-1188',
    TO_DATE('2023-11-01', 'YYYY-MM-DD'),
    'Y',
    'minhlh@company.com',
    SYSTIMESTAMP
);

INSERT INTO certificates (employee_id, cert_name, issuing_body, cert_number, issue_date, is_verified)
VALUES (
    (SELECT employee_id FROM employees WHERE email = 'lanth@company.com'),
    'AWS Certified Developer Associate',
    'Amazon Web Services',
    'AWS-DEV-77234',
    TO_DATE('2025-08-15', 'YYYY-MM-DD'),
    'N'
);

COMMIT;