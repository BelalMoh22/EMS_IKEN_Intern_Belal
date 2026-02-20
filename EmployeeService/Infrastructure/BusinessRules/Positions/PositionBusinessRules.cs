
using EmployeeService.Infrastructure.Repositories;

namespace EmployeeService.Infrastructure.BusinessRules.Positions
{
    public class PositionBusinessRules : IPositionBusinessRules
    {
        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<Department> _departmentRepository;

        public PositionBusinessRules(IRepository<Position> positionRepository, IRepository<Department> departmentRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
        }

        //public async Task ValidateAsync(decimal minSalary, decimal maxSalary, int departmentId)
        //{
        //    if (maxSalary <= minSalary)
        //        throw new Exceptions.ValidationException(new() {"Max salary must be greater than min salary."});

        //    var department = await _departmentRepository.GetByIdAsync(departmentId);
        //    if (department == null)
        //        throw new Exceptions.ValidationException(new() {$"Department with Id {departmentId} does not exist."});
        //}

        public async Task ValidateForCreateAsync(CreatePositionDto dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(dto));

            var positionNameExists = await _positionRepository.ExistsAsync("PositionName = @PositionName", new { dto.PositionName });
            if (positionNameExists)
                errors.Add($"Position Name '{dto.PositionName}' is already in use.");

            if (dto.MaxSalary <= dto.MinSalary)
                errors.Add("Max salary must be greater than min salary.");

            if (dto.MinSalary < 0 || dto.MaxSalary < 0)
                errors.Add("Salaries must be positive numbers.");

            if (dto.DepartmentId <= 0)
                errors.Add("DepartmentId must be a positive number.");

            var departmentExists = await _departmentRepository.ExistsAsync("Id = @DepartmentId", new { dto.DepartmentId });
            if (!departmentExists)
                errors.Add($"Department with Id {dto.DepartmentId} does not exist.");

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }

        public async Task ValidateForUpdateAsync(int positionId, UpdatePositionDto dto, Position existingPosition)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(dto));

            var effectivePositionName = dto.PositionName ?? existingPosition.PositionName;
            var effectiveMinSalary = dto.MinSalary != 0 ? dto.MinSalary : existingPosition.MinSalary;
            var effectiveMaxSalary = dto.MaxSalary != 0 ? dto.MaxSalary : existingPosition.MaxSalary;
            var effectiveDepartmentId = dto.DepartmentId != 0 ? dto.DepartmentId : existingPosition.DepartmentId;

            var positionNameExists = await _positionRepository.ExistsAsync(
                "PositionName = @PositionName AND Id != @Id",
                new { PositionName = effectivePositionName, Id = positionId });
            if (positionNameExists)
                errors.Add($"Position Name '{effectivePositionName}' is already in use.");

            if (effectiveMaxSalary <= effectiveMinSalary)
                errors.Add("Max salary must be greater than min salary.");

            if (effectiveMinSalary < 0 || effectiveMaxSalary < 0)
                errors.Add("Salaries must be positive numbers.");

            if (effectiveDepartmentId <= 0)
                errors.Add("DepartmentId must be a positive number.");

            var departmentExists = await _departmentRepository.ExistsAsync("Id = @DepartmentId", new { DepartmentId = effectiveDepartmentId });
            if (!departmentExists)
                errors.Add($"Department with Id {effectiveDepartmentId} does not exist.");

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }
    }
}
