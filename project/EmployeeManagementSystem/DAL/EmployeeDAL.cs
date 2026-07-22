using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class EmployeeDAL
    {
        /// <summary>
        /// Lấy danh sách nhân viên theo từ khoá tìm kiếm (tên/email) và
        /// department_id lọc tuỳ chọn. KHÔNG tự filter theo department
        /// ở đây ngoài tham số này — VPD policy employees_vpd_func đã tự
        /// lọc row theo session user đang đăng nhập, chỉ cần SELECT bình
        /// thường là đủ; filter department_id chỉ để thu hẹp trong tập
        /// hàng VPD đã cho phép (client-side convenience filter).
        /// </summary>
        public DataTable Search(OracleConnection conn, string? keyword, int? deptId)
        {
            const string sql = @"
                SELECT
                    e.employee_id, e.full_name, e.email, e.phone, e.hire_date,
                    e.job_id, j.job_title,
                    e.department_id, d.dept_name, d.dept_code,
                    e.manager_id, m.full_name AS manager_name,
                    e.location_id, l.location_name,
                    e.status
                FROM employees e
                JOIN jobs j ON e.job_id = j.job_id
                JOIN departments d ON e.department_id = d.department_id
                JOIN locations l ON e.location_id = l.location_id
                LEFT JOIN employees m ON e.manager_id = m.employee_id
                WHERE (:kw IS NULL
                       OR UPPER(e.full_name) LIKE '%' || UPPER(:kw) || '%'
                       OR UPPER(e.email) LIKE '%' || UPPER(:kw) || '%')
                  AND (:dept IS NULL OR e.department_id = :dept)
                ORDER BY e.full_name";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("kw", (object?)keyword ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("kw", (object?)keyword ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("dept", deptId.HasValue ? (object)deptId.Value : DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("dept", deptId.HasValue ? (object)deptId.Value : DBNull.Value));

            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataRow? GetById(OracleConnection conn, int employeeId)
        {
            const string sql = @"
                SELECT e.employee_id, e.full_name, e.email, e.phone, e.hire_date,
                       e.job_id, j.job_title, e.department_id, d.dept_name, e.manager_id, e.location_id, e.status
                FROM employees e
                LEFT JOIN jobs j ON e.job_id = j.job_id
                LEFT JOIN departments d ON e.department_id = d.department_id
                WHERE e.employee_id = :emp_id";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("emp_id", employeeId));
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public bool UpdateProfile(OracleConnection conn, int employeeId, string email, string? phone)
        {
            const string sql = @"
                UPDATE employees
                SET email = :email,
                    phone = :phone
                WHERE employee_id = :emp_id";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("email", email));
            cmd.Parameters.Add(new OracleParameter("phone", (object?)phone ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("emp_id", employeeId));
            
            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }


        /// <summary>
        /// INSERT nhân viên mới, trả về employee_id vừa sinh (IDENTITY).
        /// Không set created_by/created_at — cột có DEFAULT lấy từ session,
        /// tự set đúng người tạo thật, không thể giả mạo từ tầng app.
        /// </summary>
        public int Insert(OracleConnection conn, DTO.EmployeeDTO emp)
        {
            const string sql = @"
                INSERT INTO employees
                    (full_name, email, phone, hire_date, job_id, department_id, manager_id, location_id, status)
                VALUES
                    (:full_name, :email, :phone, :hire_date, :job_id, :dept_id, :manager_id, :location_id, :status)
                RETURNING employee_id INTO :new_id";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("full_name", emp.FullName));
            cmd.Parameters.Add(new OracleParameter("email", emp.Email));
            cmd.Parameters.Add(new OracleParameter("phone", (object?)emp.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("hire_date", emp.HireDate));
            cmd.Parameters.Add(new OracleParameter("job_id", emp.JobId));
            cmd.Parameters.Add(new OracleParameter("dept_id", emp.DepartmentId));
            cmd.Parameters.Add(new OracleParameter("manager_id",
                emp.ManagerId.HasValue ? emp.ManagerId.Value : DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("location_id", emp.LocationId));
            cmd.Parameters.Add(new OracleParameter("status", emp.Status));

            var outParam = new OracleParameter("new_id", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outParam);

            cmd.ExecuteNonQuery();
            return ((Oracle.ManagedDataAccess.Types.OracleDecimal)outParam.Value).ToInt32();
        }

        /// <summary>
        /// UPDATE nhân viên. Nếu user hiện tại là hr_staff_role và cố sửa
        /// department_id sang phòng của chính mình, VPD update_check sẽ
        /// chặn (ORA-28115) — để nguyên cho BLL bắt lỗi này.
        /// </summary>
        public void Update(OracleConnection conn, DTO.EmployeeDTO emp)
        {
            const string sql = @"
                UPDATE employees
                SET full_name    = :full_name,
                    email        = :email,
                    phone        = :phone,
                    hire_date    = :hire_date,
                    job_id       = :job_id,
                    department_id= :dept_id,
                    manager_id   = :manager_id,
                    location_id  = :location_id,
                    status       = :status
                WHERE employee_id = :emp_id";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("full_name", emp.FullName));
            cmd.Parameters.Add(new OracleParameter("email", emp.Email));
            cmd.Parameters.Add(new OracleParameter("phone", (object?)emp.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("hire_date", emp.HireDate));
            cmd.Parameters.Add(new OracleParameter("job_id", emp.JobId));
            cmd.Parameters.Add(new OracleParameter("dept_id", emp.DepartmentId));
            cmd.Parameters.Add(new OracleParameter("manager_id",
                emp.ManagerId.HasValue ? emp.ManagerId.Value : DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("location_id", emp.LocationId));
            cmd.Parameters.Add(new OracleParameter("status", emp.Status));
            cmd.Parameters.Add(new OracleParameter("emp_id", emp.EmployeeId));

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Soft-delete: đổi status thành TERMINATED, KHÔNG bao giờ DELETE
        /// hàng thật (đúng comment trong DDL "không DELETE nhân viên nghỉ
        /// việc"; hr_staff_role cũng không có quyền DELETE, chỉ UPDATE).
        /// </summary>
        public void SoftDelete(OracleConnection conn, int employeeId)
        {
            const string sql = "UPDATE employees SET status = 'TERMINATED' WHERE employee_id = :emp_id";
            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("emp_id", employeeId));
            cmd.ExecuteNonQuery();
        }

        // ---------- Lookup cho ComboBox ----------

        public DataTable GetJobs(OracleConnection conn)
        {
            const string sql = "SELECT job_id, job_title FROM jobs WHERE status = 'ACTIVE' ORDER BY job_title";
            using var cmd = new OracleCommand(sql, conn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetDepartments(OracleConnection conn)
        {
            const string sql = "SELECT department_id, dept_name FROM departments WHERE status = 'ACTIVE' ORDER BY dept_name";
            using var cmd = new OracleCommand(sql, conn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetLocations(OracleConnection conn)
        {
            const string sql = "SELECT location_id, location_name FROM locations WHERE status = 'ACTIVE' ORDER BY location_name";
            using var cmd = new OracleCommand(sql, conn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetManagers(OracleConnection conn)
        {
            const string sql = "SELECT employee_id, full_name FROM employees WHERE status = 'ACTIVE' ORDER BY full_name";
            using var cmd = new OracleCommand(sql, conn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}