using System;
using System.Data;
using DAL;
using Oracle.ManagedDataAccess.Client;

namespace BLL
{
    public class AuditLogService
    {
        private readonly AuditLogDAL _dal = new AuditLogDAL();

        /// <summary>
        /// Đồng bộ FGA log từ Oracle Unified Audit Trail → audit_access_log,
        /// sau đó trả về kết quả tìm kiếm theo các bộ lọc.
        /// </summary>
        public (bool Success, DataTable? Data, string? ErrorMessage) Search(
            string keyword, string action, DateTime? startDate, DateTime? endDate)
        {
            var conn = SessionManager.Connection;
            if (conn == null || conn.State != ConnectionState.Open)
                return (false, null, "Chưa đăng nhập hoặc mất kết nối.");

            try
            {
                // Đồng bộ FGA log trước khi query
                _dal.SyncFgaLog(conn);

                var data = _dal.Search(conn, keyword, action, startDate, endDate);
                return (true, data, null);
            }
            catch (OracleException ex)
            {
                return (false, null, $"Lỗi CSDL: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}
