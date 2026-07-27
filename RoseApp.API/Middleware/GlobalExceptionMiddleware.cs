using RoseApp.Application.Common;
using RoseApp.Application.Exceptions;
using System.Text.Json;

namespace RoseApp.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var traceId = context.TraceIdentifier;

            object response = ex switch
            {
                NotFoundException notFound =>
                    ResponseViewModel<object>.Fail(notFound.Message, 404),

                ValidationException validation =>
                    ResponseViewModel<object>.ValidationFail(
                        "Validation failed",
                        validation.Errors,
                        traceId),

                UnauthorizedAccessException =>
                    ResponseViewModel<object>.Fail("Unauthorized", 401),

                _ =>
                    ResponseViewModel<object>.ServerError(
                        "An unexpected error occurred",
                        traceId)
            };

            context.Response.StatusCode = ex switch
            {
                NotFoundException => 404,
                ValidationException => 422,
                UnauthorizedAccessException => 401,
                _ => 500
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }

}