namespace EmployeeService.Features.Positions.GetPositionById
{
    public record GetPositionByIdQuery(int Id) : IRequest<Position?>;
}
