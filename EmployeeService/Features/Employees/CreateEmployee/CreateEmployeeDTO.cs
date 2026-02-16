namespace EmployeeService.Features.Employees.CreateEmployee
{
    public class CreateEmployeeDTO
    {
        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public int NationalId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; } 

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public DateTime? HireDate { get; set; } = DateTime.UtcNow;

        public EmployeeStatus? Status { get; set; } = EmployeeStatus.Active;

        public int PositionId { get; set; }
    }
}
