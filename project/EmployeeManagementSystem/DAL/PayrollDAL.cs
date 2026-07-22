using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class PayrollDAL
    {
        // ══════════════════════════════════════════════════════════════
        // SEARCH — giữ nguyên
        // ══════════════════════════════════════════════════════════════
        public DataTable Search(OracleConnection conn,
                                string keyword,
                                string payPeriod,
                                string statusFilter)
        {
            const string sql = @"
                SELECT
                    p.payroll_id,
                    p.employee_id,
                    e.full_name     AS employee_name,
                    d.dept_name,
                    p.pay_period,
                    p.base_salary,
                    p.bonus,
                    p.allowances,
                    p.deductions,
                    p.net_pay,
                    p.pay_date,
                    p.approved_by,
                    p.approval_time,
                    p.status,
                    p.created_at,
                    p.created_by
                FROM payroll p
                LEFT JOIN employees e ON p.employee_id = e.employee_id
                LEFT JOIN departments d ON e.department_id = d.department_id
                WHERE (:kw IS NULL
                       OR UPPER(e.full_name) LIKE '%' || UPPER(:kw) || '%')
                  AND (:period IS NULL OR p.pay_period = :period)
                  AND (:status IS NULL OR p.status    = :status)
                ORDER BY p.pay_period DESC, e.full_name";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;

                object kwVal = keyword != null ? (object)keyword : DBNull.Value;
                object periodVal = payPeriod != null ? (object)payPeriod : DBNull.Value;
                object statusVal = statusFilter != null ? (object)statusFilter : DBNull.Value;

                cmd.Parameters.Add(new OracleParameter("kw", kwVal));
                cmd.Parameters.Add(new OracleParameter("period", periodVal));
                cmd.Parameters.Add(new OracleParameter("status", statusVal));

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════
        // GET BY ID — giữ nguyên
        // ══════════════════════════════════════════════════════════════
        public DataRow GetById(OracleConnection conn, int payrollId)
        {
            const string sql = @"
                SELECT p.payroll_id, p.employee_id, e.full_name AS employee_name,
                       p.pay_period, p.base_salary, p.bonus, p.allowances,
                       p.deductions, p.net_pay, p.pay_date,
                       p.approved_by, p.approval_time, p.status
                FROM payroll p
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.payroll_id = :pid";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("pid", payrollId));
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════
        // INSERT — giữ nguyên
        // ══════════════════════════════════════════════════════════════
        public int Insert(OracleConnection conn, PayrollDTO dto)
        {
            const string sql = @"
                INSERT INTO payroll
                    (employee_id, pay_period, base_salary, bonus,
                     allowances, deductions, pay_date, status)
                VALUES
                    (:emp_id, :period, :base, :bonus,
                     :allow, :deduct, :pay_date, :status)
                RETURNING payroll_id INTO :new_id";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;

                cmd.Parameters.Add(new OracleParameter("emp_id", OracleDbType.Int32) { Value = dto.EmployeeId });
                cmd.Parameters.Add(new OracleParameter("period", OracleDbType.Varchar2) { Value = dto.PayPeriod });
                cmd.Parameters.Add(new OracleParameter("base", OracleDbType.Decimal) { Value = dto.BaseSalary });
                cmd.Parameters.Add(new OracleParameter("bonus", OracleDbType.Decimal) { Value = dto.Bonus });
                cmd.Parameters.Add(new OracleParameter("allow", OracleDbType.Decimal) { Value = dto.Allowances });
                cmd.Parameters.Add(new OracleParameter("deduct", OracleDbType.Decimal) { Value = dto.Deductions });

                var payDateParam = new OracleParameter("pay_date", OracleDbType.Date);
                payDateParam.Value = dto.PayDate.HasValue ? (object)dto.PayDate.Value : DBNull.Value;
                cmd.Parameters.Add(payDateParam);

                cmd.Parameters.Add(new OracleParameter("status", OracleDbType.Varchar2) { Value = dto.Status });

                var outParam = new OracleParameter("new_id", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQuery();
                return ((Oracle.ManagedDataAccess.Types.OracleDecimal)outParam.Value).ToInt32();
            }
        }

        // ══════════════════════════════════════════════════════════════
        // UPDATE — ĐÃ SỬA: chặn sửa nếu phiếu không còn PENDING
        // ══════════════════════════════════════════════════════════════
        public bool Update(OracleConnection conn, PayrollDTO dto, string requestedBy)
        {
            const string plsql = @"
                DECLARE
                    v_current_status payroll.status%TYPE;
                BEGIN
                    SELECT status INTO v_current_status
                    FROM payroll
                    WHERE payroll_id = :pid
                    FOR UPDATE;

                    IF v_current_status != 'PENDING' THEN
                        RAISE_APPLICATION_ERROR(-20005,
                            'Chi duoc sua phieu luong dang PENDING. Phieu nay da ' ||
                            v_current_status || ', khong the sua.');
                    END IF;

                    INSERT INTO pending_approvals (
                        action_type, target_table, target_id, requested_by, payload, status
                    ) VALUES (
                        'SALARY_CHANGE', 'PAYROLL', :pid, :req_by, :payload, 'PENDING'
                    );
                END;";

            using (var cmd = new OracleCommand(plsql, conn))
            {
                cmd.BindByName = true;

                string payloadJson = System.Text.Json.JsonSerializer.Serialize(dto);

                cmd.Parameters.Add(new OracleParameter("pid", OracleDbType.Int32) { Value = dto.PayrollId });
                cmd.Parameters.Add(new OracleParameter("req_by", OracleDbType.Varchar2) { Value = requestedBy });
                cmd.Parameters.Add(new OracleParameter("payload", OracleDbType.Clob) { Value = payloadJson });

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        // ══════════════════════════════════════════════════════════════
        // APPROVE — ĐÃ SỬA: chặn tự duyệt phiếu do chính mình tạo (SoD)
        // ══════════════════════════════════════════════════════════════
        public bool Approve(OracleConnection conn, int payrollId)
        {
            const string plsql = @"
                DECLARE
                    v_created_by payroll.created_by%TYPE;
                    v_status     payroll.status%TYPE;
                BEGIN
                    SELECT created_by, status
                      INTO v_created_by, v_status
                      FROM payroll
                     WHERE payroll_id = :pid
                     FOR UPDATE;

                    IF v_status != 'PENDING' THEN
                        RAISE_APPLICATION_ERROR(-20001, 'Phieu luong khong o trang thai PENDING.');
                    END IF;

                    IF v_created_by = SYS_CONTEXT('USERENV','SESSION_USER') THEN
                        RAISE_APPLICATION_ERROR(-20002,
                            'Ban khong the tu duyet phieu luong do chinh minh tao (SoD).');
                    END IF;

                    UPDATE payroll
                    SET status        = 'APPROVED',
                        approved_by   = SYS_CONTEXT('USERENV','SESSION_USER'),
                        approval_time = SYSTIMESTAMP
                    WHERE payroll_id = :pid;
                END;";

            using (var cmd = new OracleCommand(plsql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("pid", payrollId));
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        // ══════════════════════════════════════════════════════════════
        // CANCEL — giữ nguyên
        // ══════════════════════════════════════════════════════════════
        public bool Cancel(OracleConnection conn, int payrollId)
        {
            const string sql = @"
                UPDATE payroll
                SET status = 'CANCELLED'
                WHERE payroll_id = :pid
                  AND status IN ('PENDING', 'APPROVED')";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("pid", payrollId));
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        // ══════════════════════════════════════════════════════════════
        // LOOKUP — giữ nguyên
        // ══════════════════════════════════════════════════════════════
        public DataTable GetEmployeeLookup(OracleConnection conn, int? departmentId = null)
        {
            // departmentId != null -> chỉ lấy nhân viên trong phòng đó (dùng cho fin_staff)
            // departmentId == null -> lấy toàn bộ (hr_staff, hr_manager, v.v.)
            const string sql = @"
                SELECT e.employee_id,
                       e.full_name || ' (' || d.dept_code || ')' AS display_name
                FROM employees e
                JOIN departments d ON e.department_id = d.department_id
                WHERE e.status = 'ACTIVE'
                  AND (:dept_id IS NULL OR e.department_id = :dept_id)
                ORDER BY e.full_name";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("dept_id",
                    departmentId.HasValue ? (object)departmentId.Value : DBNull.Value));

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