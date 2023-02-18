using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MK_Core_MVC.DB_Connection;
using MK_Core_MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// For login Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(o => o.LoginPath = new PathString("/Home/Login"));


// For binding table
builder.Services.AddDbContext<Table>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));

// 3rd step of Using Session
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddSession();

var app = builder.Build();


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

// For login Authentication
app.UseAuthentication();
app.UseAuthorization();

// Step of Using Session
//  1st-step (Inastal Pacakeg-Microsoft.AspNetCore.Session 2.2)
//  2nd-step
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
