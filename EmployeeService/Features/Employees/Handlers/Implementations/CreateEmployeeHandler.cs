namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class CreateEmployeeHandler : ICreateEmployeeHandler
    {
        private readonly IRepository<Employee> _repo;

        public CreateEmployeeHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(CreateEmployeeDTO employee)
        {
            var newEmployee = new Employee()
            {
                FirstName = employee.FirstName,
                Lastname = employee.Lastname,
                NationalId = employee.NationalId,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                Address = employee.Address,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                Status = employee.Status,
                PositionId = employee.PositionId
            };
            if (employee is null)
                throw new Exceptions.ValidationException("Employee data is required.");

            var id = await _repo.AddAsync(newEmployee);

            return id;
        }
    }
}
