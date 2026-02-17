namespace EmployeeService.Domain.Models
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string NationalId { get; set; }

        public string Email { get; set; }
    
        public int PhoneNumber { get; set; }
    
        public DateTime DateOfBirth { get; set; }
    
        public string Address { get; set; }

        public decimal Salary { get; set; }

        public DateTime? HireDate { get; set; } = DateTime.UtcNow;

        public EmployeeStatus? Status { get; set; } = EmployeeStatus.Active;

        public int PositionId { get; set; }
    }
}
