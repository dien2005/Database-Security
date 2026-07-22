using System;
using System.Data;
using DAL;

namespace BLL
{
    public class DepartmentService
    {
        private readonly DepartmentDAL _dal = new DepartmentDAL();

        public DataTable SearchDepartments(string keyword = null)
        {
            var conn = SessionManager.Connection;
            if (conn == null || conn.State != ConnectionState.Open)
                throw new InvalidOperationException("Chưa kết nối CSDL.");
            
            return _dal.Search(conn, keyword);
        }
    }
}
