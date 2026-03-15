namespace EmployeeService.Features.Employees.CreateEmployee
{
    public record CreateEmployeeCommand(CreateEmployeeDTO dto): IRequest<int>;
}
