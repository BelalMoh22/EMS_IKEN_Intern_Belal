namespace EmployeeService.Models
{
    public class Position
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public decimal MinSalary { get; set; }

        public decimal MaxSalary { get; set; }
    }
}
