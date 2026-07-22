using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using DTO;

namespace DAL
{
    public class PendingApprovalDAL
    {
        public DataTable Search(OracleConnection conn, string keyword, string status)
        {
            const string sql = @"
                SELECT 
                    approval_id,
                    action_type,
                    target_table,
                    target_id,
                    requested_by,
                    request_time,
                    payload,
                    approved_by_1,
                    approval_time_1,
                    approved_by_2,
                    approval_time_2,
                    status,
                    final_action_time,
                    notes
                FROM pending_approvals
                WHERE (:kw IS NULL OR UPPER(action_type) LIKE '%' || UPPER(:kw) || '%' OR UPPER(requested_by) LIKE '%' || UPPER(:kw) || '%')
                  AND (:st IS NULL OR status = :st)
                ORDER BY request_time DESC";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("kw", keyword != null ? (object)keyword : DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("st", status != null ? (object)status : DBNull.Value));

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════
        // APPROVE — ĐÃ SỬA: chặn tự duyệt + chặn duyệt trùng người +
        // sau khi đủ 2 người duyệt, tự động áp dụng thay đổi vào PAYROLL
        // ══════════════════════════════════════════════════════════════
        public bool Approve(OracleConnection conn, int approvalId, string approverUser)
        {
            const string plsql = @"
                DECLARE
                    v_app1           VARCHAR2(128);
                    v_app2           VARCHAR2(128);
                    v_status         VARCHAR2(20);
                    v_requested_by   VARCHAR2(128);
                    v_action_type    VARCHAR2(50);
                    v_target_table   VARCHAR2(128);
                    v_target_id      NUMBER;
                    v_payload        CLOB;
                    v_payroll_status VARCHAR2(20);
                BEGIN
                    SELECT approved_by_1, approved_by_2, status, requested_by,
                           action_type, target_table, target_id, payload
                      INTO v_app1, v_app2, v_status, v_requested_by,
                           v_action_type, v_target_table, v_target_id, v_payload
                      FROM pending_approvals
                     WHERE approval_id = :pid
                     FOR UPDATE;

                    IF v_status != 'PENDING' THEN
                        RAISE_APPLICATION_ERROR(-20001, 'Yeu cau nay da duoc xu ly.');
                    END IF;

                    IF v_requested_by = :usr THEN
                        RAISE_APPLICATION_ERROR(-20002,
                            'Ban khong the tu duyet yeu cau do chinh minh tao (SoD).');
                    END IF;

                    IF v_app1 = :usr THEN
                        RAISE_APPLICATION_ERROR(-20003,
                            'Ban da duyet buoc 1, can mot nguoi KHAC duyet buoc 2.');
                    END IF;

                    IF v_app1 IS NULL THEN
                        -- Bước 1: người đầu tiên ký
                        UPDATE pending_approvals
                        SET approved_by_1 = :usr, approval_time_1 = SYSTIMESTAMP
                        WHERE approval_id = :pid;
                    ELSE
                        -- Bước 2: đủ 2 người -> chốt APPROVED + áp dụng thay đổi thật
                        UPDATE pending_approvals
                        SET approved_by_2 = :usr, approval_time_2 = SYSTIMESTAMP,
                            status = 'APPROVED', final_action_time = SYSTIMESTAMP
                        WHERE approval_id = :pid;

                        IF v_action_type = 'SALARY_CHANGE' AND v_target_table = 'PAYROLL' THEN
                            SELECT status INTO v_payroll_status
                            FROM payroll WHERE payroll_id = v_target_id FOR UPDATE;

                            IF v_payroll_status != 'PENDING' THEN
                                RAISE_APPLICATION_ERROR(-20004,
                                    'Phieu luong khong con PENDING (hien tai: ' ||
                                    v_payroll_status || '), khong the ap dung thay doi.');
                            END IF;

                            UPDATE payroll
                            SET base_salary   = JSON_VALUE(v_payload, '$.BaseSalary'  RETURNING NUMBER),
                                bonus         = JSON_VALUE(v_payload, '$.Bonus'      RETURNING NUMBER),
                                allowances    = JSON_VALUE(v_payload, '$.Allowances' RETURNING NUMBER),
                                deductions    = JSON_VALUE(v_payload, '$.Deductions' RETURNING NUMBER),
                                status        = 'APPROVED',
                                approved_by   = SYS_CONTEXT('USERENV','SESSION_USER'),
                                approval_time = SYSTIMESTAMP
                            WHERE payroll_id = v_target_id;
                        END IF;
                    END IF;
                END;";

            using (var cmd = new OracleCommand(plsql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("pid", approvalId));
                cmd.Parameters.Add(new OracleParameter("usr", approverUser));
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool Reject(OracleConnection conn, int approvalId, string approverUser, string note)
        {
            const string sql = @"
                UPDATE pending_approvals
                SET status = 'REJECTED',
                    final_action_time = SYSTIMESTAMP,
                    notes = :note
                WHERE approval_id = :pid AND status = 'PENDING'";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("note", note != null ? (object)note : DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("pid", approvalId));

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (OracleException)
                {
                    return false;
                }
            }
        }
    }
}