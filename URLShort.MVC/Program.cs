using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using URLShort.MVC.Data;

var builder = WebApplication.CreateBuilder(args);

//  Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("UrlConnection");

//  Register the DBContext using the retrieved connection string
builder.Services.AddDbContext<UrlDBContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Route config
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Url}/{action=Shorten}/{id?}");

app.Run();
