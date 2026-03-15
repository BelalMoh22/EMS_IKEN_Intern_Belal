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
            if(request.Id <= 0)
            {
                throw new Exceptions.ValidationException(new() {"Id must be greater than zero." });
            }

            var position = await _repo.GetByIdAsync(request.Id); 
            if(position == null)
            {
                throw new Exceptions.NotFoundException($"Position with Id {request.Id} not found.");
            }
            return position;
        }
    }
}
