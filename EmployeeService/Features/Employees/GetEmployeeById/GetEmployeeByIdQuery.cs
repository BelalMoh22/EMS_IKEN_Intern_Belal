namespace EmployeeService.Features.Employees.GetEmployeeById
{
    public record GetEmployeeByIdQuery(int Id) : IRequest<Employee?>;
}
