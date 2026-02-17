namespace EmployeeService.Features.Departments.CreateDepartment
{
    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; }

        public int ManagerId { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
