namespace EmployeeService.Features.Departments.CreateDepartment
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; }

        public string? Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must contain @ and be valid.")]
        public string Email { get; set; }

        public int? ManagerId { get; set; }
    }
}
