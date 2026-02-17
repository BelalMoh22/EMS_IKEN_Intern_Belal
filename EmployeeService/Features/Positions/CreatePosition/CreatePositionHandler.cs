namespace EmployeeService.Features.Positions.CreatePosition
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;
        private readonly IRepository<Department> _departmentRepo;
        private readonly IPositionBusinessRules _rules;

        public CreatePositionHandler(IRepository<Position> repo, IRepository<Department> departmentRepo, IPositionBusinessRules rules)
        {
            _repo = repo;
            _departmentRepo = departmentRepo;
            _rules = rules;
        }

        public async Task<int> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            if (dto is null)
                throw new Exceptions.ValidationException(new() {"Position data is required." });

            ValidationHelper.ValidateModel(dto);
            await _rules.ValidateAsync(dto.MinSalary, dto.MaxSalary, dto.DepartmentId);
            var newPosition = new Position
            {
                PositionName = dto.PositionName,
                MinSalary = dto.MinSalary,
                MaxSalary = dto.MaxSalary,
                DepartmentId = dto.DepartmentId
            };

            var id = await _repo.AddAsync(newPosition);

            return id;
        }
    }
}
