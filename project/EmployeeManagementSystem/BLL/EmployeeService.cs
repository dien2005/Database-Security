using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using DTO;
using Oracle.ManagedDataAccess.Client;

namespace BLL
{
    public class EmployeeService
    {
        private readonly EmployeeDAL _dal = new EmployeeDAL();

        public (bool Success, List<EmployeeDTO> Data, string ErrorMessage) Search(string keyword, int? deptId)
        {
            try
            {
                var conn = SessionManager.Connection;
                var dt = _dal.Search(conn, string.IsNullOrWhiteSpace(keyword) ? null : keyword, deptId);

                var list = new List<EmployeeDTO>();
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new EmployeeDTO
                    {
                        EmployeeId = Convert.ToInt32(row["employee_id"]),
                        FullName     = row["full_name"] == DBNull.Value ? "" : row["full_name"].ToString(),
                        Email        = row["email"]     == DBNull.Value ? "" : row["email"].ToString(),
                        Phone        = row["phone"]     == DBNull.Value ? null : row["phone"].ToString(),
                        HireDate     = Convert.ToDateTime(row["hire_date"]),
                        JobId        = Convert.ToInt32(row["job_id"]),
                        JobTitle     = row["job_title"]    == DBNull.Value ? "" : row["job_title"].ToString(),
                        DepartmentId = Convert.ToInt32(row["department_id"]),
                        DeptName     = row["dept_name"]    == DBNull.Value ? "" : row["dept_name"].ToString(),
                        DeptCode     = row["dept_code"]    == DBNull.Value ? "" : row["dept_code"].ToString(),
                        ManagerId    = row["manager_id"]   == DBNull.Value ? (int?)null : Convert.ToInt32(row["manager_id"]),
                        ManagerName  = row["manager_name"] == DBNull.Value ? null : row["manager_name"].ToString(),
                        LocationId   = Convert.ToInt32(row["location_id"]),
                        LocationName = row["location_name"] == DBNull.Value ? "" : row["location_name"].ToString(),
                        Status       = row["status"]        == DBNull.Value ? "ACTIVE" : row["status"].ToString(),
                    });
                }
                return (true, list, null);
            }
            catch (OracleException ex)
            {
                return (false, new List<EmployeeDTO>(), "Loi truy van: " + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) AddEmployee(EmployeeDTO emp)
        {
            try
            {
                _dal.Insert(SessionManager.Connection, emp);
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                return (false, "Email đã tồn tại trong hệ thống.");
            }
            catch (OracleException ex) when (ex.Number == 2291)
            {
                return (false, "Job/Phòng ban/Địa điểm/Quản lý được chọn không hợp lệ.");
            }
            catch (OracleException ex) when (ex.Number == 1400)
            {
                return (false, "Vui lòng nhập đầy đủ các trường bắt buộc.");
            }
            catch (OracleException ex) when (ex.Number == 28101 || ex.Number == 28115)
            {
                return (false, "Bạn không có quyền thêm nhân viên vào phòng ban này (VPD từ chối).");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) UpdateEmployee(EmployeeDTO emp)
        {
            try
            {
                _dal.Update(SessionManager.Connection, emp);
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                return (false, "Email đã tồn tại trong hệ thống.");
            }
            catch (OracleException ex) when (ex.Number == 2291)
            {
                return (false, "Job/Phòng ban/Địa điểm/Quản lý được chọn không hợp lệ.");
            }
            catch (OracleException ex) when (ex.Number == 28115)
            {
                return (false, "Bạn không được chuyển nhân viên vào phòng ban của chính mình.");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
        }
        public (bool Success, string ErrorMessage) DeleteEmployee(int employeeId)
        {
            try
            {
                _dal.SoftDelete(SessionManager.Connection, employeeId);
                return (true, null);
            }
            catch (OracleException ex) when (ex.Number == 28115)
            {
                return (false, "Bạn không có quyền xoá nhân viên này.");
            }
            catch (OracleException ex)
            {
                return (false, "Loi CSDL: " + ex.Message);
            }
        }
        public List<LookupItemDTO> GetJobLookup()        { return ToLookup(_dal.GetJobs(SessionManager.Connection),        "job_id",       "job_title"); }
        public List<LookupItemDTO> GetDepartmentLookup() { return ToLookup(_dal.GetDepartments(SessionManager.Connection), "department_id", "dept_name"); }
        public List<LookupItemDTO> GetLocationLookup()   { return ToLookup(_dal.GetLocations(SessionManager.Connection),   "location_id",  "location_name"); }
        public List<LookupItemDTO> GetManagerLookup()    { return ToLookup(_dal.GetManagers(SessionManager.Connection),    "employee_id",  "full_name"); }

        private static List<LookupItemDTO> ToLookup(DataTable dt, string idCol, string textCol)
        {
            var list = new List<LookupItemDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LookupItemDTO
                {
                    Id = Convert.ToInt32(row[idCol]),
                    Text = row[textCol] == DBNull.Value ? "" : row[textCol].ToString()
                });
            }
            return list;
        }

        public (bool Success, EmployeeDTO? Data, string? ErrorMessage) GetProfile(int employeeId)
        {
            try
            {
                var conn = SessionManager.Connection;
                var row = _dal.GetById(conn, employeeId);
                if (row == null) return (false, null, "Không tìm thấy thông tin nhân viên.");

                var emp = new EmployeeDTO
                {
                    EmployeeId = Convert.ToInt32(row["employee_id"]),
                    FullName     = row["full_name"] == DBNull.Value ? "" : row["full_name"].ToString(),
                    Email        = row["email"]     == DBNull.Value ? "" : row["email"].ToString(),
                    Phone        = row["phone"]     == DBNull.Value ? null : row["phone"].ToString(),
                    HireDate     = Convert.ToDateTime(row["hire_date"]),
                    JobId        = Convert.ToInt32(row["job_id"]),
                    JobTitle     = row.Table.Columns.Contains("job_title") && row["job_title"] != DBNull.Value ? row["job_title"].ToString() : "",
                    DepartmentId = Convert.ToInt32(row["department_id"]),
                    DeptName     = row.Table.Columns.Contains("dept_name") && row["dept_name"] != DBNull.Value ? row["dept_name"].ToString() : "",
                    ManagerId    = row["manager_id"]   == DBNull.Value ? (int?)null : Convert.ToInt32(row["manager_id"]),
                    LocationId   = Convert.ToInt32(row["location_id"]),
                    Status       = row["status"]        == DBNull.Value ? "ACTIVE" : row["status"].ToString(),
                };
                return (true, emp, null);
            }
            catch (Exception ex)
            {
                return (false, null, "Lỗi lấy thông tin cá nhân: " + ex.Message);
            }
        }

        public (bool Success, string? ErrorMessage) UpdateProfile(int employeeId, string email, string? phone)
        {
            try
            {
                var conn = SessionManager.Connection;
                bool ok = _dal.UpdateProfile(conn, employeeId, email, phone);
                if (ok) return (true, null);
                return (false, "Cập nhật thất bại.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi cập nhật hồ sơ: " + ex.Message);
            }
        }
    }
}