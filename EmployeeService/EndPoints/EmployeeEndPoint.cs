
namespace EmployeeService.EndPoints
{
    public static class EmployeeEndPoint
    {
        public static async Task<RouteGroupBuilder> MapEmployeeEndPoint(this RouteGroupBuilder group)
        // this method will be used to map all employee related endpoints to the group 
        {
            group.MapGet("/", async (IEmployeeRepo _repo) =>
            {
                var employees = await _repo.GetActiveEmployeesAsync();
                return Results.Ok(employees);
            });

            group.MapGet("/{Id:int}", async (int id, IEmployeeRepo _repo) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Invalid employee id");

                var employee = await _repo.GetByIdAsync(id);
                if (employee == null)
                    return Results.NotFound();

                return Results.Ok(employee);
            });

            group.MapPost("/", async (CreateEmployeeDTO emp, IEmployeeRepo _repo) =>
            {
                if (emp == null)
                    return Results.BadRequest("Employee data is required");

                var createdId = await _repo.AddAsync(emp);

                return Results.Created($"/api/Employee/{createdId}", emp);
            });

            group.MapPut("/{id:int}", async (int id,UpdateEmployeeDTO emp,IEmployeeRepo repo) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Invalid employee id");

                var existingEmployee = await repo.GetByIdAsync(id);
                if (existingEmployee == null)
                    return Results.NotFound("Employee not found");

                var rows = await repo.UpdateAsync(id, emp);
                if (rows == 0)
                    return Results.NotFound();

                return Results.Ok(emp);
            });

            // Soft delete endpoint
            group.MapDelete("/{id:int}" , async (int id , IEmployeeRepo repo) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Invalid employee id");

                var existingEmployee = await repo.GetByIdAsync(id);
                if (existingEmployee == null)
                    return Results.NotFound("Employee not found");

                var rows = await repo.SoftDeleteAsync(id);
                if (rows == 0)
                    return Results.NotFound();

                return Results.NoContent();
            });
            return group;
        }
    }
}
