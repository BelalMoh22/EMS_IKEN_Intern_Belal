namespace EmployeeService.Features.Employees.CreateEmployee
{
    public class CreateEmployeeDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "National ID is required.")]
        [RegularExpression(@"^\d{14}$",ErrorMessage = "National ID must be exactly 14 numbers.")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must contain @ and be valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [MinimumAge(18)]
        [NotFutureDate(ErrorMessage = "Date of Birth cannot be in the future.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        public decimal Salary { get; set; }

        [NotFutureDate(ErrorMessage = "Hire Date cannot be in the future.")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public EmployeeStatus? Status { get; set; }

        [Required(ErrorMessage = "PositionId is required.")]
        public int PositionId { get; set; }
    }
}
