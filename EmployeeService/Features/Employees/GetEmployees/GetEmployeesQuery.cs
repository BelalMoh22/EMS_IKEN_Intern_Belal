namespace EmployeeService.Features.Employees.GetEmployees
{
    public record GetEmployeesQuery() : IRequest<IEnumerable<Employee>>;
}
