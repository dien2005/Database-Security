using DAL;
using DTO;

namespace BLL
{
    public class DashboardService
    {
        private readonly DashboardDAL _dal = new DashboardDAL();

        public DashboardStatsDTO LoadDashboardStats()
        {
            if (!SessionManager.IsLoggedIn)
                throw new System.InvalidOperationException("Phiên làm việc đã hết hạn, vui lòng đăng nhập lại.");

            return _dal.GetDashboardStats(SessionManager.Connection!);
        }
    }
}