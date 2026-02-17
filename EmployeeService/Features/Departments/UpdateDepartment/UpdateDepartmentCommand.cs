namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public record UpdateDepartmentCommand([FromRoute] int id, [FromBody] UpdateDepartmentDto dto) : IRequest<int>;
}
