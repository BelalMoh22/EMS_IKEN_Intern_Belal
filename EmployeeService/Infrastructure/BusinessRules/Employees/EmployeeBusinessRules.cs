namespace EmployeeService.Infrastructure.BusinessRules.Employees
{
    public class EmployeeBusinessRules : IEmployeeBusinessRules
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Position> _positionRepository;

        public EmployeeBusinessRules(IRepository<Employee> employeeRepository,IRepository<Position> positionRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task ValidateAsync(int? employeeId,string email,string nationalId,decimal salary,int positionId)
        {
            var position = await _positionRepository.GetByIdAsync(positionId);

            if (position == null)
                throw new Exceptions.ValidationException(new() { $"Position with Id {positionId} does not exist." });

            var allEmployees = await _employeeRepository.GetAllAsync();
            if (allEmployees.Any(e =>e.Email == email && (!employeeId.HasValue || e.Id != employeeId)))
                throw new Exceptions.ValidationException(new() { $"Email '{email}' is already in use." });

            if (allEmployees.Any(e =>e.NationalId == nationalId && (!employeeId.HasValue || e.Id != employeeId)))
                throw new Exceptions.ValidationException(new() { $"National ID '{nationalId}' is already in use." });

            if (salary < position.MinSalary || salary > position.MaxSalary)
                throw new Exceptions.ValidationException(new() {$"Salary must be between {position.MinSalary} and {position.MaxSalary}."});           
        }
    }
}