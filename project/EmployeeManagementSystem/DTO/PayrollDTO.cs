using System;

namespace DTO
{
    /// <summary>
    /// Đại diện 1 bản ghi PAYROLL, bao gồm cột join employee_name
    /// để hiển thị lên DataGridView mà không phải query lại.
    /// net_pay là VIRTUAL column phía DB (base + bonus + allowances - deductions).
    /// </summary>
    public class PayrollDTO
    {
        // ── PK ────────────────────────────────────────────────────────
        public int PayrollId { get; set; }

        // ── FK + Display ──────────────────────────────────────────────
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }   // JOIN từ employees.full_name
        public string DeptName { get; set; }        // JOIN từ departments.dept_name

        // ── Kỳ lương ─────────────────────────────────────────────────
        /// <summary>Định dạng YYYY-MM, ví dụ "2026-07"</summary>
        public string PayPeriod { get; set; }

        // ── Các khoản ────────────────────────────────────────────────
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }

        /// <summary>
        /// Tính client-side (mirror virtual column DB):
        /// base_salary + bonus + allowances - deductions.
        /// Không INSERT/UPDATE cột này — DB tự tính.
        /// </summary>
        public decimal NetPay { get { return BaseSalary + Bonus + Allowances - Deductions; } }

        // ── Thanh toán ───────────────────────────────────────────────
        public DateTime? PayDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovalTime { get; set; }

        // ── Trạng thái ───────────────────────────────────────────────
        /// <summary>PENDING | APPROVED | PAID | CANCELLED</summary>
        public string Status { get; set; }

        // ── Audit ────────────────────────────────────────────────────
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public PayrollDTO()
        {
            EmployeeName = string.Empty;
            DeptName     = string.Empty;
            PayPeriod    = string.Empty;
            ApprovedBy   = string.Empty;
            CreatedBy    = string.Empty;
            Status       = "PENDING";
        }
    }
}
