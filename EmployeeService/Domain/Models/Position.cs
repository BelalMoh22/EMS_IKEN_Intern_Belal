namespace EmployeeService.Domain.Models
{
    public class Position : BaseEntity
    {
        private Position() { }
        public string PositionName { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public int DepartmentId { get; set; }

        public Position(string positionName, decimal minSalary, decimal maxSalary, int departmentId)
        {
            PositionName = positionName;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
            DepartmentId = departmentId;
        }

        public void Update(string? positionName, decimal? minSalary, decimal? maxSalary, int? departmentId)
        {
            PositionName = positionName ?? PositionName;
            MinSalary = minSalary ?? MinSalary;
            MaxSalary = maxSalary ?? MaxSalary;
            DepartmentId = departmentId ?? DepartmentId;
        }
    }
}
