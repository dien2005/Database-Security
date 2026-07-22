using Oracle.ManagedDataAccess.Client;
using DTO;

namespace BLL
{
    /// <summary>
    /// Mọi DAL khác trong app dùng lại SessionManager.Connection thay vì
    /// tự mở connection mới -> đảm bảo mọi thao tác sau login đều chạy
    /// đúng dưới danh nghĩa Oracle user đã đăng nhập (giữ nguyên hiệu lực
    /// RBAC/VPD/OLS).
    /// </summary>
    public static class SessionManager
    {
        public static OracleConnection? Connection { get; set; }
        public static UserSessionDTO? CurrentUser { get; set; }
        public static bool IsLoggedIn =>
            Connection != null && Connection.State == System.Data.ConnectionState.Open;
    }
}