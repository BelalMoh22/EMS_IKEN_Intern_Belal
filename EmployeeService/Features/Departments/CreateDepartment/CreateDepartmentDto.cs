namespace EmployeeService.Features.Departments.CreateDepartment
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
    }
}
