using ContactPage.Models;
using ContactPage.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Entity Framework Configurations
builder.Services.AddDbContext<ContactDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication Set Up
builder.Services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

// Enable the memory cache
builder.Services.AddDistributedMemoryCache();

// Add the session for passing temp data

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Prevent read by java script - Securuty purpose
    options.Cookie.Name = "Contacts"; // Set unique cookie name
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Time out session in 5 minutes
    options.Cookie.IsEssential = true; // Cookies required to work
});


var app = builder.Build();

// Create DB and add default data
using var _dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ContactDbContext>();
try
{
    _dbContext.Database.Migrate();
    DbInitializer.SeedData(_dbContext);
}
catch (Exception ex)
{
    return;
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Contact}/{id?}");

app.Run();
