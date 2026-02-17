namespace EmployeeService.Features.Departments
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
        {
            GetDepartmentsEndpoint.MapEndpoint(app);
            GetDepartmentByIdEndpoint.MapEndpoint(app);
            CreateDepartmentEndpoint.MapEndpoint(app);
            UpdateDepartmentEndpoint.MapEndpoint(app);
            DeleteDepartmentEndpoint.MapEndpoint(app);
        }
    }
}
