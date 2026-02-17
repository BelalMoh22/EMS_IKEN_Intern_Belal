namespace EmployeeService.Features.Positions
{
    public static class PositionEndpoints
    {
        public static void MapPositionEndpoints(this IEndpointRouteBuilder app)
        {
            GetPositionsEndpoint.MapEndpoint(app);
            GetPositionByIdEndpoint.MapEndpoint(app);
            CreatePositionEndpoint.MapEndpoint(app);
            UpdatePositionEndpoint.MapEndpoint(app);
            DeletePositionEndpoint.MapEndpoint(app);
        }
    }
}
