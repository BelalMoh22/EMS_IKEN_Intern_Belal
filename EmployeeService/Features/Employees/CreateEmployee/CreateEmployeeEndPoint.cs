using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Features.Employees.CreateEmployee
{
    public static class CreateEmployeeEndPoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async ([FromBody] CreateEmployeeCommand command, [FromServices] IMediator mediator) =>
            {
                var id = await mediator.Send(command);
                return Results.Created($"/employees/{id}", id);
            }).WithName("CreateEmployee").WithTags("Employees");
        }
    }
}
