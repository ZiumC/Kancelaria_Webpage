using GB_Webpage.Middlewares;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using GB_Webpage.Data;
using GB_Webpage.Services;
using GB_Webpage.Services.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddDbContext<ApiContext>(options => options.UseInMemoryDatabase("ArticleDb"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("pl-PL");

    options.AddSupportedCultures(new string[] { "en-US", "pl-PL", "de-DE" });
    options.AddSupportedUICultures(new string[] { "en-US", "pl-PL", "de-DE" });

    options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };

});

builder.Services.AddRazorPages();
builder.Services.AddLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseRequestLocalization();

app.Use(async (context, next) =>
{

    var contexRequestQuery = context.Request.Query;
    string cultureQueryRequestString = context.Request.Query["culture"].ToString();

    if (contexRequestQuery.Count() > 0 && !cultureQueryRequestString.Equals(""))
    {

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureQueryRequestString);

        context.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureQueryRequestString)),
            new CookieOptions() { Expires = DateTime.Now.AddYears(1) }
            );


        context.Response.Redirect(context.Request.Headers["Referer"].ToString());

    }

    await next.Invoke();
});

app.UseMiddleware<RedirectWhenCultureMismatch>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//removing default controller
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.Run();
