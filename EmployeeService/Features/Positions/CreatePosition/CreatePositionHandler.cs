namespace EmployeeService.Features.Positions.CreatePosition
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;
        private readonly IPositionBusinessRules _rules;

        public CreatePositionHandler(IRepository<Position> repo, IPositionBusinessRules rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public async Task<int> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            await _rules.ValidateForCreateAsync(dto);

            var Position = new Position(
                dto.PositionName,
                dto.MinSalary,
                dto.MaxSalary,
                dto.DepartmentId
            );

            return await _repo.AddAsync(Position);
        }
    }
}
