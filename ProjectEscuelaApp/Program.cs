global using WebEscuelaProject.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddLocalization();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddMvcCore(options =>
{
    options.Filters.Add(new NoCacheAttribute());
});


var app = builder.Build();

var supportedCultures = new[]{
    new CultureInfo("es-CR")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-CR"),
    SupportedCultures = supportedCultures,
    FallBackToParentCultures = false
});

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("es-CR");

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
