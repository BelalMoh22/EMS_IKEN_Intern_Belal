namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public record UpdateDepartmentCommand([FromRoute] int Id, [FromBody] UpdateDepartmentDto dto) : IRequest<int>;
}
