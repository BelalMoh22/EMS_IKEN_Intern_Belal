namespace EmployeeService.Features.Positions.UpdatePosition
{
    public class UpdatePositionHandler : IRequestHandler<UpdatePositionCommand, int>
    {
        private readonly IRepository<Position> _repo;
        private readonly IRepository<Department> _departmentRepo;
        private readonly IPositionBusinessRules _rules;

        public UpdatePositionHandler(IRepository<Position> repo, IRepository<Department> departmentRepo, IPositionBusinessRules rules)
        {
            _repo = repo;
            _departmentRepo = departmentRepo;
            _rules = rules;
        }

        public async Task<int> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException(new() { "Invalid position Id." });

            var existingPosition = await _repo.GetByIdAsync(request.id);

            if (existingPosition == null)
                throw new NotFoundException($"Position with Id {request.id} not found.");

            ValidationHelper.ValidateModel(request.dto);

            // Determine effective values: use provided ones, otherwise keep existing
            var effectiveMinSalary = request.dto.MinSalary != 0 ? request.dto.MinSalary : existingPosition.MinSalary;
            var effectiveMaxSalary = request.dto.MaxSalary != 0 ? request.dto.MaxSalary : existingPosition.MaxSalary;
            var effectiveDepartmentId = request.dto.DepartmentId != 0 ? request.dto.DepartmentId : existingPosition.DepartmentId;
            await _rules.ValidateAsync(effectiveMinSalary, effectiveMaxSalary, effectiveDepartmentId);

            existingPosition.PositionName = request.dto.PositionName ?? existingPosition.PositionName;
            existingPosition.MinSalary = effectiveMinSalary;
            existingPosition.MaxSalary = effectiveMaxSalary;
            existingPosition.DepartmentId = effectiveDepartmentId;

            var rows = await _repo.UpdateAsync(request.id, existingPosition);
            return rows;
        }
    }
}
