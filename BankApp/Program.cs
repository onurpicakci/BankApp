using Application.Interfaces;
using Application.Services;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repository;
using Domain.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountActivitiesService, AccountActivitiesService>();
builder.Services.AddScoped<IAccountActivitiesRepository, AccountActivitiesRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<BankAppDbContext>();

builder.Services.AddAuthentication("YourUserAuthenticationScheme")
    .AddCookie("YourUserAuthenticationScheme", options =>
{
    options.Cookie.Name = "BankAppUserCookie";
    options.LoginPath = "/User/Login";
});


builder.Services.AddAuthentication
        (CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "BankAppCookie";
            options.LoginPath = "/CustomerList/Index"; 
            options.LogoutPath = "/Login/Logout";
        });

builder.Services.AddMvc();
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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();