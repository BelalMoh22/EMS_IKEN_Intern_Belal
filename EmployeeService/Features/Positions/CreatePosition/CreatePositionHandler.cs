
namespace EmployeeService.Features.Positions.CreatePosition
{
    public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;
        private readonly IRepository<Department> _departmentRepo;

        public CreatePositionHandler(IRepository<Position> repo, IRepository<Department> departmentRepo)
        {
            _repo = repo;
            _departmentRepo = departmentRepo;
        }

        public async Task<int> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            if (dto is null)
                throw new Exceptions.ValidationException("Position data is required.");

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
