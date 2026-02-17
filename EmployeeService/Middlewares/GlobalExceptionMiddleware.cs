namespace EmployeeService.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exceptions.ValidationException ex)
            {
                await HandleExceptionAsync(context, 400, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, 404, ex.Message);
            }
            //catch (Exception ex)
            //{
            //    await HandleExceptionAsync(context, 500, "Internal Server Error");
            //}
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, 500, ex.Message);
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<string>.FailureResponse(
                new List<string> { message },
                "Request Failed"
            );
            
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
