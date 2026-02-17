namespace EmployeeService.Features.Departments
{
    public static class DepartmentEndpoint
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
