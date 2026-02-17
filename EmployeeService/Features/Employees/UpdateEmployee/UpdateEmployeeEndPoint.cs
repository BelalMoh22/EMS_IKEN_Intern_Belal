namespace EmployeeService.Features.Employees.UpdateEmployee
{
    public static class UpdateEmployeeEndPoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{id:int}", async ([FromRoute] int id, [FromBody] UpdateEmployeeDTO dto, [FromServices] IMediator mediator) =>
            {
                var command = new UpdateEmployeeCommand(id , dto);
                var rows = await mediator.Send(command);
                var response = ApiResponse<int>.SuccessResponse("Employee updated successfully");
                return Results.Ok(response);
            }).WithDescription("Updating an existing Employee").WithTags("Employees");
        }
    }
}
