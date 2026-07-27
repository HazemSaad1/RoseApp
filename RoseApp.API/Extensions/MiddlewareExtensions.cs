using RoseApp.API.Middleware;

namespace RoseApp.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(
            this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            return app;
        }
    }
}
