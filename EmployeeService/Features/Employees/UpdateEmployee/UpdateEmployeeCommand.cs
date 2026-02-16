namespace EmployeeService.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeCommand([FromRoute] int id , [FromBody]UpdateEmployeeDTO dto): IRequest<int>;
}
