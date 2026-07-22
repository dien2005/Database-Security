namespace DTO
{
    public class UserSessionDTO
    {
        public string Username { get; set; }
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public string FullName { get; set; }
        public bool IsValidEmployee { get; set; }
        public bool IsHrManager { get; set; }
        public bool IsHrStaff { get; set; }
        public bool IsDeptManager { get; set; }
        public bool IsFinStaff { get; set; }
    }
}