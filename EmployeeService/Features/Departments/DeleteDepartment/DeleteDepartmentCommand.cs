namespace EmployeeService.Features.Departments.DeleteDepartment
{
    public record DeleteDepartmentCommand(int id) : IRequest<int>;
}
