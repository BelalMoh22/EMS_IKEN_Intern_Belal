
using EmployeeService.Domain.Models;
using EmployeeService.Infrastructure.Repositories;

namespace EmployeeService.Infrastructure.BusinessRules.Departments
{
    public class DepartmentBusinessRules : IDepartmentBusinessRules
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public DepartmentBusinessRules(IRepository<Department> departmentRepository, IRepository<Employee> employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        //public async Task ValidateAsync(int managerId)
        //{
        //    if (managerId <= 0)
        //        throw new Exceptions.ValidationException(new() {"ManagerId must be a positive employee Id."});

        //    var manager = await _employeeRepository.GetByIdAsync(managerId);
        //    if (manager == null)
        //        throw new Exceptions.ValidationException(new() {$"Manager with Id {managerId} does not exist."});
        //}

        public async Task ValidateForCreateAsync(CreateDepartmentDto dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(dto));

            var departmentNameExists = await _employeeRepository.ExistsAsync("DepartmentName = @DepartmentName", new { dto.DepartmentName });
            if (departmentNameExists)
                errors.Add($"Department Name '{dto.DepartmentName}' is already in use.");

            var emailExists = await _departmentRepository.ExistsAsync("Email = @Email", new { dto.Email });
            if (emailExists)
                errors.Add($"Email '{dto.Email}' is already in use.");

            if (dto.ManagerId <= 0)
                throw new Exceptions.ValidationException(new() { "ManagerId must be a positive employee Id." });

            var managerId = await _employeeRepository.ExistsAsync("Id = @ManagerId", new { dto.ManagerId });
            if (!managerId)
                errors.Add($"Manager with Id {dto.ManagerId} does not exist.");

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }

        public async Task ValidateForUpdateAsync(int departmentId, UpdateDepartmentDto dto, Department existingDepartment)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(dto));

            var effectiveDepartmentName = dto.DepartmentName ?? existingDepartment.DepartmentName;
            var effectiveEmail = dto.Email ?? existingDepartment.Email;
            var effectiveManagerId = dto.ManagerId != 0 ? dto.ManagerId : existingDepartment.ManagerId;

            var departmentNameExists = await _departmentRepository.ExistsAsync("DepartmentName = @DepartmentName AND Id != @Id",
                new { DepartmentName = effectiveDepartmentName, Id = departmentId });
            if (departmentNameExists)
                errors.Add($"Department Name '{effectiveDepartmentName}' is already in use.");

            var emailExists = await _departmentRepository.ExistsAsync(
                "Email = @Email AND Id != @Id",
                new { Email = effectiveEmail, Id = departmentId });
            if (emailExists)
                errors.Add($"Email '{effectiveEmail}' is already in use.");

            var manager = await _employeeRepository.GetByIdAsync(effectiveManagerId);
            if (manager == null)
            {
                errors.Add($"Manager with Id {effectiveManagerId} does not exist.");
            }
            else
            {
                var managerAlreadyAssigned = await _departmentRepository.ExistsAsync(
                    "ManagerId = @ManagerId AND Id != @DepartmentId",
                    new { ManagerId = effectiveManagerId, DepartmentId = departmentId });

                if (managerAlreadyAssigned)
                    errors.Add($"Employee with Id {effectiveManagerId} is already assigned as a manager to another department.");
            }

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);
        }
    }
}
