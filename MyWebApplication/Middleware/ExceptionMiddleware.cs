using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await WriteErrorResponse(context, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteErrorResponse(context, "Internal Server Error");
            }
        }

        private Task WriteErrorResponse(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                error = message,
                path = context.Request.Path,
                timestamp = DateTime.UtcNow
            });
            return context.Response.WriteAsync(result);
        }
    }
}
