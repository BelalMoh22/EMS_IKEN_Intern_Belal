namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentDto
    {
        public string DepartmentName { get; set; }

        public int ManagerId { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
