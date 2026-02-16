namespace EmployeeService.Features.Employees
{
    public static class EmployeesEndpoints
    {
        public static void MapEmployeesEndpoints(this IEndpointRouteBuilder app)
        {
            CreateEmployeeEndPoint.MapEndpoint(app);
            UpdateEmployeeEndPoint.MapEndpoint(app);
            DeleteEmployeeEndPoint.MapEndpoint(app);
            //GetEmployeesEndpoint.MapEndpoint(app);
            //GetEmployeeByIdEndpoint.MapEndpoint(app);
        }
    }
}
