namespace EmployeeService.Features.Employees.GetEmployeeById
{
    public static class GetEmployeeByIdEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id:int}", async ([FromServices] IMediator mediator, [FromRoute]int id) =>
            {
                var command = new GetEmployeeByIdQuery(id);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).WithName("GetEmployeeById").WithTags("Employees");
        }
    }
}
