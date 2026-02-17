namespace EmployeeService.Features.Departments.GetDepartments
{
    public static class GetDepartmentsEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async ([FromServices] IMediator mediator) =>
            {
                var command = new GetDepartmentsQuery();
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).WithName("GetDepartments").WithTags("Departments");
        }
    }
}
