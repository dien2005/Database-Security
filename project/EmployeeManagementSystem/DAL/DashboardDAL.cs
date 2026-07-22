using System;
using Oracle.ManagedDataAccess.Client;
using DTO;

namespace DAL
{
    public class DashboardDAL
    {
        public int GetTotalCount(OracleConnection conn)
        {
            string sql = "SELECT COUNT(employee_id) FROM employees";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int GetCountByStatus(OracleConnection conn, string status)
        {
            string sql = "SELECT COUNT(employee_id) FROM employees WHERE status = :status";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("status", status));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public DashboardStatsDTO GetDashboardStats(OracleConnection conn)
        {
            return new DashboardStatsDTO
            {
                TotalEmployees = GetTotalCount(conn),
                ActiveEmployees = GetCountByStatus(conn, "ACTIVE"),
                InactiveEmployees = GetCountByStatus(conn, "INACTIVE"),
                TerminatedEmployees = GetCountByStatus(conn, "TERMINATED")
            };
        }
    }
}