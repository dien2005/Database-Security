using DAL;
using DTO;
using Oracle.ManagedDataAccess.Client;
using System;

namespace BLL
{
    public class AuthService
    {
        private readonly AuthDAL _authDal = new AuthDAL();

        public (bool Success, UserSessionDTO? Session, string? ErrorMessage) Login(string username, string password)
        {
            OracleConnection? conn = null;
            try
            {
                conn = OracleConnectionFactory.CreateAndOpen(username, password);
                var ctx = _authDal.GetSessionContext(conn);

                var session = new UserSessionDTO
                {
                    Username = username.ToUpper(),
                    IsValidEmployee = ctx?["is_valid_emp"]?.ToString() == "Y",
                    IsHrManager = ctx?["is_hr_manager"]?.ToString() == "Y",
                    IsHrStaff = ctx?["is_hr_staff"]?.ToString() == "Y",
                    IsDeptManager = ctx?["is_dept_manager"]?.ToString() == "Y",
                    IsFinStaff = ctx?["is_fin_staff"]?.ToString() == "Y",
                };

                if (session.IsValidEmployee && int.TryParse(ctx?["employee_id"]?.ToString(), out int empId))
                {
                    session.EmployeeId = empId;
                    var emp = _authDal.GetEmployeeInfo(conn, empId);
                    session.FullName = emp?["full_name"]?.ToString();
                }

                if (int.TryParse(ctx?["department_id"]?.ToString(), out int deptId))
                    session.DepartmentId = deptId;

                SessionManager.Connection = conn;
                SessionManager.CurrentUser = session;
                return (true, session, null);
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                conn?.Dispose();
                return (false, null, "Sai tên đăng nhập hoặc mật khẩu.");
            }
            catch (OracleException ex) when (ex.Number == 28000)
            {
                conn?.Dispose();
                return (false, null, "Tài khoản đã bị khoá. Liên hệ quản trị viên.");
            }
            catch (OracleException ex)
            {
                conn?.Dispose();
                return (false, null, $"Lỗi kết nối CSDL: {ex.Message}");
            }
            catch (Exception ex)
            {
                conn?.Dispose();
                return (false, null, $"Lỗi không xác định: {ex.Message}");
            }
        }

        public void Logout()
        {
            SessionManager.Connection?.Dispose();
            SessionManager.Connection = null;
            SessionManager.CurrentUser = null;
        }
    }
}