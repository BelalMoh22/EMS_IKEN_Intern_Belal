namespace EmployeeService.Features.Positions.UpdatePosition
{
    public class UpdatePositionHandler : IRequestHandler<UpdatePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;
        private readonly IRepository<Department> _departmentRepo;

        public UpdatePositionHandler(IRepository<Position> repo, IRepository<Department> departmentRepo)
        {
            _repo = repo;
            _departmentRepo = departmentRepo;
        }

        public async Task<int> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException("Invalid position Id.");

            var existingPosition = await _repo.GetByIdAsync(request.id);

            if (existingPosition == null)
                throw new NotFoundException($"Position with Id {request.id} not found.");

            // Map DTO → Existing Entity
            existingPosition.PositionName = request.dto.PositionName;
            existingPosition.MinSalary = request.dto.MinSalary;
            existingPosition.MaxSalary = request.dto.MaxSalary;
            existingPosition.DepartmentId = request.dto.DepartmentId;

            var rows = await _repo.UpdateAsync(request.id, existingPosition);

            return rows;
        }
    }
}
