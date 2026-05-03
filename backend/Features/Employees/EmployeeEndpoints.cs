namespace backend.Features.Employees
{
    public static class EmployeesEndpoints
    {
        public static RouteGroupBuilder MapEmployeesEndpoints(this RouteGroupBuilder group)
        {
            CreateEmployeeEndPoint.MapEndpoint(group).RequireAuthorization("EmployeeCRUD");
            UpdateEmployeeEndPoint.MapEndpoint(group).RequireAuthorization("EmployeeCRUD");
            DeleteEmployeeEndPoint.MapEndpoint(group).RequireAuthorization("EmployeeCRUD");
            GetEmployeesEndpoint.MapEndpoint(group).RequireAuthorization("ReadResource");
            GetEmployeeByIdEndpoint.MapEndpoint(group).RequireAuthorization("ReadResource");
            GetEmployeeByUserIdEndpoint.MapEndpoint(group).RequireAuthorization();

            return group;
        }
    }
}
