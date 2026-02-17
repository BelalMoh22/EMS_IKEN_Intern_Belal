namespace EmployeeService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<Position>, PositionRepository>();
            //builder.Services.AddScoped<IGetEmployeesHandler, GetEmployeesHandler>();
            //builder.Services.AddScoped<IGetEmployeeByIdHandler, GetEmployeeByIdHandler>();
            //builder.Services.AddScoped<ICreateEmployeeHandler, CreateEmployeeHandler>();
            //builder.Services.AddScoped<IUpdateEmployeeHandler, UpdateEmployeeHandler>();
            //builder.Services.AddScoped<IDeleteEmployeeHandler, DeleteEmployeeHandler>();
            // Use Auth
            builder.Services.AddAuthorization();

            // Add DbConnection and ApplicationName
            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register MediatR services
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();
            // Log application startup information
            app.Logger.LogTrace("Application is starting (Trace)");
            app.Logger.LogInformation("Application started successfully (Information)");
            app.Logger.LogWarning("This is a sample warning during startup (Warning)");
            try
            {
                if (string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")))
                {
                    throw new Exception("Connection string missing");
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogCritical(ex, "Critical error during startup");
            }

            // Log application name and connection string for debugging
            //app.Logger.LogInformation("App Name: {AppName}", appName);
            //app.Logger.LogInformation("Connection String: {ConnectionString}", connectionString);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Best Order for Middleware: Exception Handling, Logging, Authentication, Authorization
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // /health endpoint 
            //app.MapGet("/api/health", () => Results.Ok("First EndPoint From EMS"));
            app.MapGroup("/api/employees").MapEmployeesEndpoints();
            app.MapGroup("/api/departments").MapDepartmentEndpoints();
            app.MapGroup("/api/positions").MapPositionEndpoints();

            app.Run();
        }
    }
}
