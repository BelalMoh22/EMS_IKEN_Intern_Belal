namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, int>
    {
        private readonly IRepository<Department> _repo;
        private readonly IDepartmentBusinessRules _rules;

        public UpdateDepartmentHandler(IRepository<Department> repo, IDepartmentBusinessRules rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public async Task<int> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException(new() {"Invalid department Id." });

            var existingDepartment = await _repo.GetByIdAsync(request.id);
            if (existingDepartment == null)
                throw new NotFoundException($"Department with Id {request.id} not found.");

            ValidationHelper.ValidateModel(request.dto);

            var effectiveManagerId = request.dto.ManagerId != 0 ? request.dto.ManagerId : existingDepartment.ManagerId;
            await _rules.ValidateAsync(effectiveManagerId);

            existingDepartment.DepartmentName = request.dto.DepartmentName ?? existingDepartment.DepartmentName;
            existingDepartment.ManagerId = effectiveManagerId;

            var rows = await _repo.UpdateAsync(request.id, existingDepartment);
            return rows;
        }
    }
}
