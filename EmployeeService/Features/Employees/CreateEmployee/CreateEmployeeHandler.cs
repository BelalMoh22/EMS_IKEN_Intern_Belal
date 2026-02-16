namespace EmployeeService.Features.Employees.CreateEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;

        public DeleteEmployeeHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var newEmployee = new Employee()
            {
                FirstName = request.dto.FirstName,
                Lastname = request.dto.Lastname,
                NationalId = request.dto.NationalId,
                Email = request.dto.Email,
                PhoneNumber = request.dto.PhoneNumber,
                DateOfBirth = request.dto.DateOfBirth,
                Address = request.dto.Address,
                Salary = request.dto.Salary,
                HireDate = request.dto.HireDate,
                Status = request.dto.Status,
                PositionId = request.dto.PositionId
            };
            if (request.dto is null)
                throw new Exceptions.ValidationException("Employee data is required.");

            var id = await _repo.AddAsync(newEmployee);

            return id;
        }
    }
}
