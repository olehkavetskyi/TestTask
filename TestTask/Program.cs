using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TestTask.Data;
using TestTask.Interfaces;
using TestTask.Models;
using TestTask.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot/Angular";
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"])),
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidateAudience = true,
        };
    });


builder.Services.AddIdentity<User, ApplicationRole>()
    .AddEntityFrameworkStores<UrlShortenerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Url>, Repository<Url>>();
builder.Services.AddScoped<IRepository<ConfigurableText>, Repository<ConfigurableText>>();
builder.Services.AddScoped<IUrlService, UrlService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var repo = services.GetRequiredService<IRepository<ConfigurableText>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        await UrlShortenerDbContextSeed.SeedUsers(userManager, roleManager);
        await UrlShortenerDbContextSeed.SeedText(repo, userManager);
        await UrlShortenerDbContextSeed.SeedUrls(services, 5);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while seeding the data: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=About}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapFallbackToController("AngularEntryPoint", "Home");
});

app.Run();
