namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentDto
    {
        public string? DepartmentName { get; set; }

        public string? Description { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email must contain @ and be valid.")]
        public string? Email { get; set; }

        public int? ManagerId { get; set; }

        public bool? IsActive { get; set; }
    }
}
