namespace backend.Features.Employees.GetEmployees
{
    public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<Employee>>
    {
        private readonly EmployeeRepository _repo;

        public GetEmployeesHandler(EmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Employee>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserRole == Roles.HR.ToString())
            {
                var all = await _repo.GetAllAsync();
                // HR can see everyone except Master
                return all.Where(e => e.User == null || e.User.Role != Roles.Master);
            }

            if (request.UserRole == Roles.Master.ToString())
            {
                // Master can only view HR employees
                var all = await _repo.GetAllAsync();
                return all.Where(e => e.User != null && e.User.Role == Roles.HR);
            }

            if (request.UserRole == Roles.Manager.ToString())
            {
                var manager = await _repo.GetByUserIdAsync(request.UserId);
                if (manager == null) return Enumerable.Empty<Employee>();

                var team = await _repo.GetEmployeesByManagerIdAsync(manager.Id);
                // Manager can see team except Master
                return team.Where(e => e.User == null || e.User.Role != Roles.Master);
            }
            return Enumerable.Empty<Employee>();
        }
    }
}