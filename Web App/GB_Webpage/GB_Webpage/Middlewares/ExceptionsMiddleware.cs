using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GB_Webpage.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch
            {
                httpContext.Response.Redirect("/error.html");
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsMiddleware>();
        }
    }
}
