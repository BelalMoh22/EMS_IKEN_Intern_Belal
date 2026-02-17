namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class UpdateEmployeeHandler
    : IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly IEmployeeBusinessRules _rules;

        public UpdateEmployeeHandler(
            IRepository<Employee> repo,
            IEmployeeBusinessRules rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public async Task<int> Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new Exceptions.ValidationException(
                    new() { "Invalid employee Id." });

            var existingEmployee = await _repo.GetByIdAsync(request.Id);

            if (existingEmployee == null)
                throw new NotFoundException($"Employee with Id {request.Id} not found.");

            var dto = request.dto;

            // Validate only provided fields via DataAnnotations
            ValidationHelper.ValidateModel(dto);

            // Determine effective values for business rules (use existing when dto is null)
            var effectiveEmail = dto.Email ?? existingEmployee.Email;
            var effectiveNationalId = dto.NationalId ?? existingEmployee.NationalId;
            var effectiveSalary = dto.Salary ?? existingEmployee.Salary;
            var effectivePositionId = dto.PositionId ?? existingEmployee.PositionId;

            await _rules.ValidateAsync(request.Id, effectiveEmail, effectiveNationalId, effectiveSalary, effectivePositionId);

            // Apply only the fields that were provided
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                existingEmployee.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.Lastname))
                existingEmployee.Lastname = dto.Lastname;
            if (!string.IsNullOrWhiteSpace(dto.NationalId))
                existingEmployee.NationalId = dto.NationalId;
            if (!string.IsNullOrWhiteSpace(dto.Email))
                existingEmployee.Email = dto.Email;
            if (dto.PhoneNumber.HasValue)
                existingEmployee.PhoneNumber = dto.PhoneNumber.Value;
            if (dto.DateOfBirth.HasValue)
                existingEmployee.DateOfBirth = dto.DateOfBirth.Value;
            if (!string.IsNullOrWhiteSpace(dto.Address))
                existingEmployee.Address = dto.Address;
            if (dto.Salary.HasValue)
                existingEmployee.Salary = dto.Salary.Value;
            if (dto.HireDate.HasValue)
                existingEmployee.HireDate = dto.HireDate.Value;
            if (dto.Status.HasValue)
                existingEmployee.Status = dto.Status;
            if (dto.PositionId.HasValue)
                existingEmployee.PositionId = dto.PositionId.Value;

            return await _repo.UpdateAsync(request.Id, existingEmployee);
        }
    }
}
