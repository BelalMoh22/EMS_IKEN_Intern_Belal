using EmployeeService.Models;
using EmployeeService.Repositories.Implementations;
using EmployeeService.Repositories.Interfaces;

namespace EmployeeService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>();
            builder.Services.AddScoped<IGenericRepo<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IGenericRepo<Position>, PositionRepository>();
            // Use Auth
            builder.Services.AddAuthorization();

            // Add DbConnection and ApplicationName
            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var app = builder.Build();

            // Log application name and connection string for debugging
            app.Logger.LogInformation("App Name: {AppName}", appName);
            app.Logger.LogInformation("Connection String: {ConnectionString}", connectionString);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // /health endpoint 
            app.MapGet("/api/health", () => Results.Ok("First EndPoint From EMS"));

            app.Run();
        }
    }
}
