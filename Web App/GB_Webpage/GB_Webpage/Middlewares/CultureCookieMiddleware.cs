using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace GB_Webpage.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CultureCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] supportedCultures = new string[] { "pl-PL", "de-DE", "en-US" };

        public CultureCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            SetCookie(httpContext);

            return _next(httpContext);
        }

        private void SetCookie(HttpContext context)
        {

            var contexRequestQuery = context.Request.Query;
            string cultureQueryRequestString = context.Request.Query["culture"].ToString();

            if (contexRequestQuery.Count() > 0 && !cultureQueryRequestString.Equals(""))
            {

                //default culture is pl-PL
                var selectedCulture = supportedCultures[0];

                bool isSupportedCulture = supportedCultures
                    .Where(sc => sc.ToLower() == cultureQueryRequestString.ToLower())
                    .Count() == 1;

                if (isSupportedCulture)
                {
                    selectedCulture = cultureQueryRequestString;
                }

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedCulture);

                context.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(selectedCulture)),
                    new CookieOptions() { Expires = DateTime.Now.AddYears(1) }
                    );


                context.Response.Redirect(context.Request.Headers["Referer"].ToString());

            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CultureCookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseCultureCookieMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureCookieMiddleware>();
        }
    }
}
