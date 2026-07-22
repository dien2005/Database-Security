using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DAL;
using DTO;
using Oracle.ManagedDataAccess.Client;

namespace BLL
{
    public class PayrollService
    {
        private readonly PayrollDAL _dal = new PayrollDAL();

        public (bool Success, List<PayrollDTO> Data, string ErrorMessage)
            Search(string keyword, string payPeriod, string status)
        {
            try
            {
                var conn = SessionManager.Connection;
                string kw = string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim();
                string pd = string.IsNullOrWhiteSpace(payPeriod) ? null : payPeriod.Trim();
                string st = (status == null || status.StartsWith("--")) ? null : status;

                var dt = _dal.Search(conn, kw, pd, st);
                var list = new List<PayrollDTO>();
                foreach (DataRow row in dt.Rows)
                    list.Add(MapRow(row));

                return (true, list, null);
            }
            catch (OracleException ex)
            {
                return (false, new List<PayrollDTO>(), "Loi truy van: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, new List<PayrollDTO>(), "Loi: " + ex.Message);
            }
        }

        public (bool Success, int NewId, string ErrorMessage) AddPayroll(PayrollDTO dto)
        {
            try
            {
                string validErr;
                if (!ValidateDTO(dto, out validErr))
                    return (false, 0, validErr);

                int newId = _dal.Insert(SessionManager.Connection, dto);
                return (true, newId, null);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                return (false, 0, "Nhan vien nay da co phieu luong ky " + dto.PayPeriod + ".");
            }
            catch (OracleException ex) when (ex.Number == 2291)
            {
                return (false, 0, "Nhan vien khong hop le.");
            }
            catch (OracleException ex) when (ex.Number == 2290)
            {
                return (false, 0, "Du lieu khong hop le: " + ex.Message);
            }
            catch (OracleException ex) when (ex.Number == 28115 || ex.Number == 28101)
            {
                return (false, 0, "Ban khong co quyen tao phieu luong cho nhan vien nay.");
            }
            catch (OracleException ex)
            {
                return (false, 0, "Loi CSDL: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, 0, "Loi: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════════
        // UPDATE — ĐÃ SỬA: bắt thêm lỗi -20005 (phiếu không còn PENDING)
        // ══════════════════════════════════════════════════════════════
        public (bool Success, string ErrorMessage) UpdatePayroll(PayrollDTO dto)
        {
            try
            {
                string validErr;
                if (!ValidateDTO(dto, out validErr))
                    return (false, validErr);

                string currentUser = SessionManager.CurrentUser?.Username ?? "UNKNOWN";
                _dal.Update(SessionManager.Connection, dto, currentUser);
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 20005)
            {
                return (false, "Chi duoc sua phieu luong dang PENDING. Phieu nay da duoc xu ly roi.");
            }
            catch (OracleException ex) when (ex.Number == 2290)
            {
                return (false, "Du lieu khong hop le: " + ex.Message);
            }
            catch (OracleException ex) when (ex.Number == 28115)
            {
                return (false, "Ban khong co quyen cap nhat phieu luong nay.");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Loi: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════════
        // APPROVE — ĐÃ SỬA: bắt thêm lỗi -20001 (không PENDING) và
        // -20002 (tự duyệt phiếu do chính mình tạo)
        // ══════════════════════════════════════════════════════════════
        public (bool Success, string ErrorMessage) ApprovePayroll(int payrollId)
        {
            try
            {
                _dal.Approve(SessionManager.Connection, payrollId);
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 20001)
            {
                return (false, "Phieu luong khong o trang thai PENDING (co the da duoc xu ly).");
            }
            catch (OracleException ex) when (ex.Number == 20002)
            {
                return (false, "Ban khong the tu duyet phieu luong do chinh minh tao (SoD).");
            }
            catch (OracleException ex) when (ex.Number == 28115)
            {
                return (false, "Ban khong co quyen phe duyet phieu luong nay.");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Loi: " + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) CancelPayroll(int payrollId)
        {
            try
            {
                bool cancelled = _dal.Cancel(SessionManager.Connection, payrollId);
                if (!cancelled)
                    return (false, "Khong the huy: Phieu da PAID hoac CANCELLED roi.");
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 28115)
            {
                return (false, "Ban khong co quyen huy phieu luong nay.");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Loi: " + ex.Message);
            }
        }

        public List<LookupItemDTO> GetEmployeeLookup()
        {
            // fin_staff chỉ được xem nhân viên trong phòng của mình
            int? deptFilter = null;
            var user = SessionManager.CurrentUser;
            if (user != null && user.IsFinStaff && !user.IsHrStaff && !user.IsHrManager)
                deptFilter = user.DepartmentId;

            var dt = _dal.GetEmployeeLookup(SessionManager.Connection, deptFilter);
            var list = new List<LookupItemDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LookupItemDTO
                {
                    Id = Convert.ToInt32(row["employee_id"]),
                    Text = row["display_name"] != null ? row["display_name"].ToString() : ""
                });
            }
            return list;
        }

        private static PayrollDTO MapRow(DataRow row)
        {
            return new PayrollDTO
            {
                PayrollId = Convert.ToInt32(row["payroll_id"]),
                EmployeeId = Convert.ToInt32(row["employee_id"]),
                EmployeeName = row["employee_name"] == DBNull.Value ? "" : row["employee_name"].ToString(),
                DeptName = row["dept_name"] == DBNull.Value ? "" : row["dept_name"].ToString(),
                PayPeriod = row["pay_period"] == DBNull.Value ? "" : row["pay_period"].ToString(),
                BaseSalary = row["base_salary"] == DBNull.Value ? 0 : Convert.ToDecimal(row["base_salary"]),
                Bonus = row["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(row["bonus"]),
                Allowances = row["allowances"] == DBNull.Value ? 0 : Convert.ToDecimal(row["allowances"]),
                Deductions = row["deductions"] == DBNull.Value ? 0 : Convert.ToDecimal(row["deductions"]),
                PayDate = row["pay_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["pay_date"]),
                ApprovedBy = row["approved_by"] == DBNull.Value ? "" : row["approved_by"].ToString(),
                ApprovalTime = row["approval_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["approval_time"]),
                Status = row["status"] == DBNull.Value ? "PENDING" : row["status"].ToString(),
                CreatedAt = row["created_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] == DBNull.Value ? "" : row["created_by"].ToString()
            };
        }

        private static bool ValidateDTO(PayrollDTO dto, out string error)
        {
            if (dto.EmployeeId <= 0)
            {
                error = "Vui long chon nhan vien.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(dto.PayPeriod) ||
                !Regex.IsMatch(dto.PayPeriod, @"^\d{4}-(0[1-9]|1[0-2])$"))
            {
                error = "Ky luong phai co dinh dang YYYY-MM (VD: 2026-07).";
                return false;
            }
            if (dto.BaseSalary <= 0)
            {
                error = "Luong co ban phai lon hon 0.";
                return false;
            }
            if (dto.Bonus < 0)
            {
                error = "Thuong khong duoc am.";
                return false;
            }
            if (dto.Allowances < 0)
            {
                error = "Phu cap khong duoc am.";
                return false;
            }
            if (dto.Deductions < 0)
            {
                error = "Khau tru khong duoc am.";
                return false;
            }
            error = null;
            return true;
        }
    }
}