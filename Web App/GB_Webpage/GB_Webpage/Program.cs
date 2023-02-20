var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options
    .AddSupportedCultures(new string[] { "pl-PL", "en-US", "de-DE" })
    .AddSupportedUICultures(new string[] { "pl-PL", "en-US", "de-DE" });
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
