namespace EmployeeService.Features.Positions.UpdatePosition
{
    public record UpdatePositionCommand([FromRoute] int id, [FromBody] UpdatePositionDto dto) : IRequest<int>;
}
