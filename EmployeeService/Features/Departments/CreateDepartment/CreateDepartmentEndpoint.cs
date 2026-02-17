namespace EmployeeService.Features.Departments.CreateDepartment
{
    public static class CreateDepartmentEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async ([FromBody] CreateDepartmentDto dto, [FromServices] IMediator mediator) =>
            {
                var command = new CreateDepartmentCommand(dto);
                var id = await mediator.Send(command);

                var response = ApiResponse<int>.SuccessResponse("Department created successfully");

                return Results.Created($"/departments/{id}", response);
            }).WithName("CreateDepartment").WithTags("Departments");
        }
    }
}
