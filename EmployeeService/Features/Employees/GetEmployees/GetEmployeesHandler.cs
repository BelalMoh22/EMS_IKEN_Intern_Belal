namespace EmployeeService.Features.Employees.GetEmployees
{
    public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<Employee>>
    {
        private readonly IRepository<Employee> _repo;

        public GetEmployeesHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
