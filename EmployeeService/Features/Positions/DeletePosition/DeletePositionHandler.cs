namespace EmployeeService.Features.Positions.DeletePosition
{
    public class DeletePositionHandler : IRequestHandler<DeletePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;

        public DeletePositionHandler(IRepository<Position> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException(new() {"Invalid position Id." });

            var rows = await _repo.DeleteAsync(request.id);
            if (rows == 0)
                throw new NotFoundException($"Position with Id {request.id} not found.");

            return rows;
        }
    }
}
