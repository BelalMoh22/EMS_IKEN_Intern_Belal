namespace EmployeeService.Features.Employees.GetEmployeeById
{
    public class GetEmployeeByIdhandler : IRequestHandler<GetEmployeeByIdQuery, Employee?>
    {
        private readonly IRepository<Employee> _repo;

        public GetEmployeeByIdhandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.Id <= 0) 
                throw new Exceptions.ValidationException(new() { "Id must be greater than 0." });
            
            var employee = await _repo.GetByIdAsync(request.Id);
            if(employee == null)
                throw new Exceptions.NotFoundException($"Employee with Id {request.Id} not found.");

            return employee;
        }
    }
}