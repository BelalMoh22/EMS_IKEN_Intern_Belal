namespace EmployeeService.Features.Positions.DeletePosition
{
    public record DeletePositionCommand(int id) : IRequest<int>;
}
