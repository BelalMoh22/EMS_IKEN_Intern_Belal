using System.Text.Json.Serialization;

namespace EmployeeService.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeCommand(int Id, UpdateEmployeeDTO dto) : IRequest<int>;

}
