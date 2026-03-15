namespace EmployeeService.Features.Departments.GetDepartmentById
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<Department?>;
}
