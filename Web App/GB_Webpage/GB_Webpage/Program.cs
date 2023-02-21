using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options
    .AddSupportedCultures(new string[] { "en-US", "pl-PL", "de-DE" })
    .AddSupportedUICultures(new string[] { "en-US", "pl-PL", "de-DE" });

    options.SetDefaultCulture("pl-PL");

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
