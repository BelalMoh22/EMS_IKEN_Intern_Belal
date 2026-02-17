namespace EmployeeService.Features.Positions.GetPositionById
{
    public class GetPositionByIdHandler : IRequestHandler<GetPositionByIdQuery, Position?>
    {
        private readonly IRepository<Position> _repo;

        public GetPositionByIdHandler(IRepository<Position> repo)
        {
            _repo = repo;
        }

        public async Task<Position?> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}
