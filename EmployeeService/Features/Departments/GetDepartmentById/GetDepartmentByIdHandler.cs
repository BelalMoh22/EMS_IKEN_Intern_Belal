namespace EmployeeService.Features.Departments.GetDepartmentById
{
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, Department?>
    {
        private readonly IRepository<Department> _repo;

        public GetDepartmentByIdHandler(IRepository<Department> repo)
        {
            _repo = repo;
        }

        public async Task<Department?> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}
