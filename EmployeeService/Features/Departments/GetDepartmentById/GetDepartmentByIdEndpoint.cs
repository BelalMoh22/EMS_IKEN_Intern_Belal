namespace EmployeeService.Features.Departments.GetDepartmentById
{
    public static class GetDepartmentByIdEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id:int}", async ([FromServices] IMediator mediator, [FromRoute] int id) =>
            {
                var command = new GetDepartmentByIdQuery(id);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).WithName("GetDepartmentById").WithTags("Departments");
        }
    }
}
