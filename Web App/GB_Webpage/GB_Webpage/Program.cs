using GB_Webpage.Middlewares;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using GB_Webpage.Data;
using GB_Webpage.Services;
using GB_Webpage.Services.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddDbContext<ApiContext>(options => options.UseInMemoryDatabase("ArticleDb"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("pl-PL");

    var supportedCultures = new string[] { "pl-PL", "de-DE", "en-US" };

    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };

});

builder.Services.AddRazorPages();
builder.Services.AddLocalization();

//builder.Services.AddLogging(loggingBuilder => {
//    loggingBuilder.AddFile("app.log", append: true);
//});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    var issuer = builder.Configuration["profiles:GB_Webpage:applicationUrl"].Split(";")[0];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1),
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretSignatureKey"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }

            return Task.CompletedTask;
        }
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();

app.UseMiddleware<CultureCookieMiddleware>();
app.UseMiddleware<CultureMismatchMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.Run();

