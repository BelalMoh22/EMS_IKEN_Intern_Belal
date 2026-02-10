namespace EmployeeService.Models
{
    public class Position : BaseEntity
    {
        public string PositionName { get; set; }

        public decimal MinSalary { get; set; }

        public decimal MaxSalary { get; set; }

        public int DepartmentId { get; set; }
    }
}
