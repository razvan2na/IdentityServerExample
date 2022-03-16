using AuthServer.Configs;
using AuthServer.Data;
using AuthServer.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password Requirements
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Account/Login";
    config.LogoutPath = "/Account/Logout";
});

// IdentityServer Configuration
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddOperationalStore(options =>
        options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")))
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddAspNetIdentity<ApplicationUser>();

// CORS
var _corsPolicy = "AllowAll";

builder.Services.AddCors(options => 
    options.AddPolicy(_corsPolicy, p => p
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// HTTP Request Pipeline

app.UseStaticFiles();
app.UseCors(_corsPolicy);

app.UseRouting();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
