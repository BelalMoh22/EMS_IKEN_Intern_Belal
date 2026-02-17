namespace EmployeeService.Features.Departments.CreateDepartment
{
    public static class CreateDepartmentEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async ([FromBody] CreateDepartmentCommand command, [FromServices] IMediator mediator) =>
            {
                var id = await mediator.Send(command);

                var response = ApiResponse<int>.SuccessResponse(id, "Department created successfully");

                return Results.Created($"/departments/{id}", response);
            }).WithName("CreateDepartment").WithTags("Departments");
        }
    }
}
