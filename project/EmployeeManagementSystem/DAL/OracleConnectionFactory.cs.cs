using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    /// <summary>
    /// Tạo connection Oracle bằng ĐÚNG username/password người dùng nhập ở
    /// LoginForm — không dùng chung 1 connection cố định cho toàn app.
    /// Nhờ vậy mọi RBAC/VPD/OLS đã thiết lập ở tầng DB tự động có hiệu lực
    /// đúng theo danh nghĩa người đang đăng nhập, không cần code C# tự lọc quyền.
    /// </summary>
    public static class OracleConnectionFactory
    {
        private static string DataSource
        {
            get
            {
                var ds = ConfigurationManager.AppSettings["OracleDataSource"];
                if (string.IsNullOrWhiteSpace(ds))
                    throw new ConfigurationErrorsException(
                        "Thiếu key 'OracleDataSource' trong App.config.");
                return ds;
            }
        }

        /// <summary>
        /// Mở connection mới. Nếu sai username/password, Oracle sẽ ném
        /// OracleException (ORA-01017) — KHÔNG bắt ở đây, để tầng BLL
        /// quyết định message hiển thị phù hợp cho từng loại lỗi.
        /// </summary>
        public static OracleConnection CreateAndOpen(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username không được để trống.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password không được để trống.", nameof(password));

            string connStr = $"User Id={username};Password={password};Data Source={DataSource};";
            var conn = new OracleConnection(connStr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Dùng khi cần mở connection mới bằng credential đã lưu (vd sau khi
        /// SessionManager.Connection bị đóng do timeout, cần mở lại mà không
        /// bắt người dùng nhập lại mật khẩu trong phiên làm việc hiện tại).
        /// Cân nhắc kỹ trước khi dùng — ưu tiên dùng lại SessionManager.Connection.
        /// </summary>
        public static bool TestConnection(string username, string password, out string? errorMessage)
        {
            errorMessage = null;
            try
            {
                using var conn = CreateAndOpen(username, password);
                return true;
            }
            catch (OracleException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}