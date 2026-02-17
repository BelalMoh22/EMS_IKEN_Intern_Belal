namespace EmployeeService.Infrastructure.BusinessRules.Departments
{
    public class DepartmentBusinessRules : IDepartmentBusinessRules
    {
        private readonly IRepository<Employee> _employeeRepository;

        public DepartmentBusinessRules(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task ValidateAsync(int managerId)
        {
            if (managerId <= 0)
            {
                throw new Exceptions.ValidationException(new()
                {
                    "ManagerId must be a positive employee Id."
                });
            }

            var manager = await _employeeRepository.GetByIdAsync(managerId);

            if (manager == null)
            {
                throw new Exceptions.ValidationException(new()
                {
                    $"Manager with Id {managerId} does not exist."
                });
            }
        }
    }
}
