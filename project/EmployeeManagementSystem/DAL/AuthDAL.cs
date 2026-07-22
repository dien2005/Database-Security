using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class AuthDAL
    {
        /// <summary>
        /// Đọc toàn bộ Application Context EMP_CTX của session hiện tại
        /// (đã được logon trigger set_emp_ctx_trg tự động set lúc connection
        /// vừa mở thành công). Đây là nguồn xác định role/quyền chính xác
        /// nhất, khớp 100% với những gì VPD/OLS đang enforce cho user này.
        /// </summary>
        public DataRow? GetSessionContext(OracleConnection conn)
        {
            const string sql = @"
                SELECT
                    SYS_CONTEXT('EMP_CTX','IS_VALID_EMP')    AS is_valid_emp,
                    SYS_CONTEXT('EMP_CTX','EMPLOYEE_ID')     AS employee_id,
                    SYS_CONTEXT('EMP_CTX','DEPARTMENT_ID')   AS department_id,
                    SYS_CONTEXT('EMP_CTX','IS_HR_MANAGER')   AS is_hr_manager,
                    SYS_CONTEXT('EMP_CTX','IS_HR_STAFF')     AS is_hr_staff,
                    SYS_CONTEXT('EMP_CTX','IS_DEPT_MANAGER') AS is_dept_manager,
                    SYS_CONTEXT('EMP_CTX','IS_FIN_STAFF')    AS is_fin_staff
                FROM dual";

            using var cmd = new OracleCommand(sql, conn);
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// Lấy tên đầy đủ + mã phòng ban để hiển thị lên MainForm sau khi login.
        /// Dùng đúng employee_id lấy từ context ở trên (không tự nhập tay),
        /// tránh lệch dữ liệu giữa context và bảng thật.
        /// </summary>
        public DataRow? GetEmployeeInfo(OracleConnection conn, int employeeId)
        {
            const string sql = @"
                SELECT e.full_name, e.email, d.dept_code, d.dept_name
                FROM employees e
                JOIN departments d ON e.department_id = d.department_id
                WHERE e.employee_id = :emp_id";

            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("emp_id", employeeId));
            using var adapter = new OracleDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}