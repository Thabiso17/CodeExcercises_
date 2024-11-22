using CodeExcercises.Back.Calculator;
using CodeExcercises.Back.Services;
using CodeExcercises.Data;
using CodeExcercises.Interfaces;
using CodeExcercises.Interfaces.Calculator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IStringCalculator, StringCalculator>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserRegistration, UserRegistrationService>();
builder.Services.AddLogging();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add session services
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Ensure session middleware is added before UseAuthorization and UseEndpoints

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();