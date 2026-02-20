using EmployeeService.Domain.Models;

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
            if (request.Id <= 0)
                throw new Exceptions.ValidationException(new() {"Invalid department Id." });

            var existingDepartment = await _repo.GetByIdAsync(request.Id);
            if (existingDepartment == null)
                throw new NotFoundException($"Department with Id {request.Id} not found.");

            var dto = request.dto;
            await _rules.ValidateForUpdateAsync(request.Id, dto, existingDepartment);

            existingDepartment.Update(
                dto.DepartmentName,
                dto.Description,
                dto.Email,
                dto.ManagerId
            );

            return await _repo.UpdateAsync(request.Id, existingDepartment);
        }
    }
}
