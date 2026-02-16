namespace EmployeeService.Features.Employees.GetEmployeeById
{
    public class GetEmployeeByIdhandler
    {
        public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee?>
        {
            private readonly IRepository<Employee> _repo;

            public GetEmployeeByIdQueryHandler(IRepository<Employee> repo)
            {
                _repo = repo;
            }

            public async Task<Employee?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
            {
                return await _repo.GetByIdAsync(request.Id);
            }
        }
    }
}
