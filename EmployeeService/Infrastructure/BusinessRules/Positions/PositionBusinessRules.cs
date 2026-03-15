namespace EmployeeService.Infrastructure.BusinessRules.Positions
{
    public sealed class PositionBusinessRules : IPositionBusinessRules
    {
        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public PositionBusinessRules(IRepository<Position> positionRepository,IRepository<Department> departmentRepository,IRepository<Employee> employeeRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task ValidateForCreateAsync(CreatePositionDto dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(dto));

            var nameExists = await _positionRepository.ExistsAsync("PositionName = @PositionName",new { dto.PositionName });
            if (nameExists)
                errors.Add($"Position name '{dto.PositionName}' already exists.");

            if (dto.MaxSalary <= dto.MinSalary)
                errors.Add("Max salary must be greater than min salary.");

            if (dto.DepartmentId <= 0)
            {
                errors.Add("DepartmentId must be a positive number.");
            }
            else
            {
                var departmentExists = await _departmentRepository.ExistsAsync("Id = @DepartmentId", new { dto.DepartmentId });
                if (!departmentExists)
                    errors.Add($"Department with Id {dto.DepartmentId} does not exist.");
            }

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }

        public async Task ValidateForUpdateAsync(int positionId,UpdatePositionDto dto,Position existingPosition)
        {
            var errors = new List<string>();

            var effectiveName =dto.PositionName ?? existingPosition.PositionName;

            var effectiveMinSalary =dto.MinSalary ?? existingPosition.MinSalary;

            var effectiveMaxSalary =dto.MaxSalary ?? existingPosition.MaxSalary;

            var effectiveDepartmentId =dto.DepartmentId ?? existingPosition.DepartmentId;

            var nameExists = await _positionRepository.ExistsAsync(
                "PositionName = @PositionName AND Id != @Id",
                new { PositionName = effectiveName, Id = positionId });
            if (nameExists)
                errors.Add($"Position name '{effectiveName}' already exists.");

            if (effectiveMinSalary < 0 || effectiveMaxSalary < 0)
                errors.Add("Salaries must be positive numbers.");

            if (effectiveMaxSalary <= effectiveMinSalary)
                errors.Add("Max salary must be greater than min salary.");

            if (effectiveDepartmentId <= 0)
            {
                errors.Add("DepartmentId must be a positive number.");
            }
            else
            {
                var departmentExists = await _departmentRepository.ExistsAsync(
                    "Id = @DepartmentId",
                    new { DepartmentId = effectiveDepartmentId });

                if (!departmentExists)
                    errors.Add($"Department with Id {effectiveDepartmentId} does not exist.");
            }

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }

        public async Task ValidateForDeleteAsync(int positionId)
        {
            var errors = new List<string>();

            var isAssignedToEmployees = await _employeeRepository.ExistsAsync("PositionId = @PositionId",new { PositionId = positionId });
            if (isAssignedToEmployees)
                errors.Add("Cannot delete position because it is assigned to one or more employees.");

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }
    }
}