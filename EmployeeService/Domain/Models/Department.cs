namespace EmployeeService.Domain.Models
{
    public class Department : BaseEntity
    {
        private Department() { }

        public string DepartmentName { get; private set; }
        public string? Description { get; private set; }
        public string Email { get; private set; }
        public int? ManagerId { get; private set; }
        public bool? IsActive { get; private set; } = true;

        public Department(string departmentName, string? description, string email, int? managerId)
        {
            DepartmentName = departmentName;
            Description = description ?? string.Empty;
            Email = email;
            ManagerId = managerId;
        }

        public void Update(string? departmentName, string? description, string? email, int? managerId , bool? isActive)
        {
            DepartmentName = departmentName ?? DepartmentName;
            Description = description ?? Description;
            Email = email ?? Email;
            ManagerId = managerId ?? ManagerId;
            IsActive = isActive ?? IsActive;
        }
    }
}
