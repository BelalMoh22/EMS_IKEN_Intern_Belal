namespace EmployeeService.Features.Positions.UpdatePosition
{
    public class UpdatePositionDto
    {
        public string PositionName { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public int DepartmentId { get; set; }
    }
}
