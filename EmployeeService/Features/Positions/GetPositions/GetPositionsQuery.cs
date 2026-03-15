namespace EmployeeService.Features.Positions.GetPositions
{
    public record GetPositionsQuery() : IRequest<IEnumerable<Position>>;
}
