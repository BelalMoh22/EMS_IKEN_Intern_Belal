namespace EmployeeService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container(DI).
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<Position>, PositionRepository>();
            builder.Services.AddScoped<IEmployeeBusinessRules, EmployeeBusinessRules>();
            builder.Services.AddScoped<IPositionBusinessRules, PositionBusinessRules>();
            builder.Services.AddScoped<IDepartmentBusinessRules, DepartmentBusinessRules>();
            // Use Auth
            builder.Services.AddAuthorization();

            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

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
            app.MapGroup("/api/employees").MapEmployeesEndpoints();
            app.MapGroup("/api/departments").MapDepartmentEndpoints();
            app.MapGroup("/api/positions").MapPositionEndpoints();

            app.Run();
        }
    }
}
