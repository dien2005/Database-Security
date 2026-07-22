using System;

namespace DTO
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        
        public int? ParentDeptId { get; set; }
        public string ParentDeptName { get; set; }
        
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
