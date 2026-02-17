using EmployeeService.Domain.Interfaces;
using EmployeeService.Domain.Models;
using EmployeeService.Exceptions;

namespace EmployeeService.Infrastructure.BusinessRules.Positions
{
    public class PositionBusinessRules : IPositionBusinessRules
    {
        private readonly IRepository<Department> _departmentRepository;

        public PositionBusinessRules(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task ValidateAsync(decimal minSalary, decimal maxSalary, int departmentId)
        {
            if (maxSalary <= minSalary)
            {
                throw new Exceptions.ValidationException(new()
                {
                    "Max salary must be greater than min salary."
                });
            }

            var department = await _departmentRepository.GetByIdAsync(departmentId);

            if (department == null)
            {
                throw new Exceptions.ValidationException(new()
                {
                    $"Department with Id {departmentId} does not exist."
                });
            }
        }
    }
}
