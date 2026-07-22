using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class AuditLogDAL
    {
        /// <summary>
        /// Gọi procedure SYNC_FGA_AUDIT_LOG để đồng bộ FGA log từ
        /// Oracle Unified Audit Trail vào bảng audit_access_log.
        /// Nếu user không có quyền EXECUTE thì bỏ qua silently.
        /// </summary>
        public void SyncFgaLog(OracleConnection conn)
        {
            try
            {
                using var cmd = new OracleCommand("hr_admin.sync_fga_audit_log", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Silently ignore nếu user không có quyền EXECUTE
            }
        }

        /// <summary>
        /// Tìm kiếm audit log với các bộ lọc: keyword, action, khoảng ngày.
        /// Columns: audit_id, event_time, db_user, object_name, policy_name, action_name, sql_text
        /// </summary>
        public DataTable Search(OracleConnection conn, string keyword, string action,
                                DateTime? startDate, DateTime? endDate)
        {
            var dt = new DataTable();

            string sql = @"
                SELECT audit_id,
                       event_time,
                       db_user,
                       object_schema,
                       object_name,
                       policy_name,
                       action_name,
                       sql_text,
                       sync_time
                FROM hr_admin.audit_access_log
                WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += @" AND (UPPER(db_user)     LIKE '%' || UPPER(:kw) || '%'
                           OR  UPPER(object_name) LIKE '%' || UPPER(:kw2) || '%'
                           OR  UPPER(policy_name) LIKE '%' || UPPER(:kw3) || '%'
                           OR  UPPER(sql_text)    LIKE '%' || UPPER(:kw4) || '%')";
            }
            if (!string.IsNullOrWhiteSpace(action))
            {
                sql += " AND UPPER(action_name) = UPPER(:actionType)";
            }
            if (startDate.HasValue)
            {
                sql += " AND event_time >= :startDate";
            }
            if (endDate.HasValue)
            {
                sql += " AND event_time < :endDate";
            }

            sql += " ORDER BY event_time DESC";

            using var cmd = new OracleCommand(sql, conn);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                cmd.Parameters.Add(new OracleParameter("kw",  keyword));
                cmd.Parameters.Add(new OracleParameter("kw2", keyword));
                cmd.Parameters.Add(new OracleParameter("kw3", keyword));
                cmd.Parameters.Add(new OracleParameter("kw4", keyword));
            }
            if (!string.IsNullOrWhiteSpace(action))
            {
                cmd.Parameters.Add(new OracleParameter("actionType", action));
            }
            if (startDate.HasValue)
            {
                cmd.Parameters.Add(new OracleParameter("startDate", OracleDbType.Date)).Value
                    = startDate.Value.Date;
            }
            if (endDate.HasValue)
            {
                cmd.Parameters.Add(new OracleParameter("endDate", OracleDbType.Date)).Value
                    = endDate.Value.Date.AddDays(1);
            }

            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
