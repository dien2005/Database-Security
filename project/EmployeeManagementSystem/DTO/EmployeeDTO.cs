using System;

namespace DTO
{
    /// <summary>
    /// Đại diện 1 dòng employee, bao gồm cả cột join hiển thị lên UI
    /// (dept_name, job_title, location_name, manager_name) để không phải
    /// query lại nhiều lần khi bind lên DataGridView.
    /// </summary>
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime HireDate { get; set; }

        public int JobId { get; set; }
        public string? JobTitle { get; set; }

        public int DepartmentId { get; set; }
        public string? DeptName { get; set; }
        public string? DeptCode { get; set; }

        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }

        // ACTIVE | INACTIVE | TERMINATED — không dùng DELETE thật
        public string Status { get; set; } = "ACTIVE";
    }
}