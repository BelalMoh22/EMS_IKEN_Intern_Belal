namespace EmployeeService.Models
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }

        public int ManagerId { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
