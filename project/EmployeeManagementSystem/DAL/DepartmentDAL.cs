using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using DTO;

namespace DAL
{
    public class DepartmentDAL
    {
        public DataTable Search(OracleConnection conn, string keyword)
        {
            const string sql = @"
                SELECT 
                    d.department_id,
                    d.dept_name,
                    d.dept_code,
                    d.manager_id,
                    m.full_name AS manager_name,
                    d.location_id,
                    l.location_name,
                    d.parent_dept_id,
                    p.dept_name AS parent_dept_name,
                    d.status,
                    d.created_at
                FROM departments d
                LEFT JOIN employees m ON d.manager_id = m.employee_id
                LEFT JOIN locations l ON d.location_id = l.location_id
                LEFT JOIN departments p ON d.parent_dept_id = p.department_id
                WHERE (:kw IS NULL OR UPPER(d.dept_name) LIKE '%' || UPPER(:kw) || '%' OR UPPER(d.dept_code) LIKE '%' || UPPER(:kw) || '%')
                ORDER BY d.dept_name";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                
                object kwVal = keyword != null ? (object)keyword : DBNull.Value;
                cmd.Parameters.Add(new OracleParameter("kw", kwVal));

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
