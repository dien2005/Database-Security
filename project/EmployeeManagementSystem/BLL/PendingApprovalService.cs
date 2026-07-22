using System;
using System.Data;
using DAL;

namespace BLL
{
    public class PendingApprovalService
    {
        private readonly PendingApprovalDAL _dal = new PendingApprovalDAL();

        public DataTable Search(string keyword = null, string status = null)
        {
            var conn = SessionManager.Connection;
            if (conn == null || conn.State != ConnectionState.Open)
                throw new InvalidOperationException("Chưa kết nối CSDL.");

            if (status == "ALL" || (status != null && status.StartsWith("--")))
                status = null;

            return _dal.Search(conn, keyword, status);
        }

        // ══════════════════════════════════════════════════════════════
        // APPROVE — ĐÃ SỬA: không nuốt exception nữa, phân loại rõ từng
        // mã lỗi để người dùng biết chính xác vì sao duyệt thất bại
        // ══════════════════════════════════════════════════════════════
        public (bool Success, string Message) Approve(int approvalId)
        {
            var conn = SessionManager.Connection;
            if (conn == null || conn.State != ConnectionState.Open)
                return (false, "Chưa kết nối CSDL.");

            string currentUser = SessionManager.CurrentUser?.Username ?? "UNKNOWN";

            try
            {
                _dal.Approve(conn, approvalId, currentUser);
                return (true, "Đã duyệt thành công.");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 20001)
            {
                return (false, "Yêu cầu này đã được xử lý trước đó.");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 20002)
            {
                return (false, "Bạn không thể tự duyệt yêu cầu do chính mình tạo (SoD).");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 20003)
            {
                return (false, "Bạn đã duyệt bước 1 rồi, cần người khác duyệt bước 2.");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 20004)
            {
                return (false, "Phiếu lương không còn ở trạng thái PENDING, không thể áp dụng thay đổi.");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 28115 || ex.Number == 28101)
            {
                return (false, "Bạn không có quyền thực hiện thao tác này.");
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                return (false, "Lỗi CSDL: " + ex.Message);
            }
        }

        public (bool Success, string Message) Reject(int approvalId, string note)
        {
            var conn = SessionManager.Connection;
            if (conn == null || conn.State != ConnectionState.Open)
                return (false, "Chưa kết nối CSDL.");

            string currentUser = SessionManager.CurrentUser?.Username ?? "UNKNOWN";
            string finalNote = $"Rejected by {currentUser}. Lydo: {note}";

            bool result = _dal.Reject(conn, approvalId, currentUser, finalNote);
            if (result)
                return (true, "Đã từ chối yêu cầu thành công!");
            else
                return (false, "Lỗi khi từ chối yêu cầu.");
        }
    }
}