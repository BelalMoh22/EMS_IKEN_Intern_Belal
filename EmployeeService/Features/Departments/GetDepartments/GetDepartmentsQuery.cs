namespace EmployeeService.Features.Departments.GetDepartments
{
    public record GetDepartmentsQuery() : IRequest<IEnumerable<Department>>;
}
