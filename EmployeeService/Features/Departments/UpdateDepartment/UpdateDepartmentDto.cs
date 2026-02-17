namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; }

        public int ManagerId { get; set; }
    }
}
