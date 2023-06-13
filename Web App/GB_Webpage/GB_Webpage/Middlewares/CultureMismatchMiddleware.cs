using Microsoft.AspNetCore.Http.Extensions;

namespace GB_Webpage.Middlewares
{
    public class CultureMismatchMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _subPage = "/news";
        private readonly string _cultureAllowed = "pl-PL";


        public CultureMismatchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string currentURL = GetCurrentURL(httpContext);
            string currentCulture = GetCurrentUICulture();

            //if someone is in 'news' fold and his UI culture is different than pl-PL, this person
            //is redirected to index because 'news' fold should be avaiable only in pl-PL culture
            if (currentURL.Contains(_subPage) && !currentCulture.Contains(_cultureAllowed))
            {
                httpContext.Response.Redirect("/");
            }

            await _next(httpContext);
        }


        private string GetCurrentUICulture()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString();
        }

        private string GetCurrentURL(HttpContext context)
        {

            return context.Request.GetDisplayUrl();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RedirectToIndexExtensions
    {
        public static IApplicationBuilder UseRedirectToIndex(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMismatchMiddleware>();
        }
    }
}
