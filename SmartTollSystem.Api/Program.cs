using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity;
using SmartTollSystem.Domain.Entities.Identity;
using SmartTollSystem.Infrastructure.Data;
using System;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => {
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
})
 .AddXmlSerializerFormatters();

// Identity setup
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); //Generates description for all endpoints
//builder.Services.AddSwaggerGen(); //generates OpenAPI specification



builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "VehicleOwner" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
    }

    // Default admin user
    var adminEmail = "admin@toll.com";
    var adminPassword = "Admin123!";

    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new ApplicationUser
        {
            FullName="YahiaSheriif",
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}


app.UseAuthentication();
app.MapControllers();

app.Run();
