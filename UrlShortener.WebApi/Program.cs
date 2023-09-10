using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.Core;
using UrlShortener.Core.Entities;
using UrlShortener.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore();
builder.Services.AddInfrastructure();
builder.Services.AddCors();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie("Cookies");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await roleManager.CreateAsync(new IdentityRole("Admin"));

    await userManager.CreateAsync(new AppUser
    {
        UserName = "Test",
    }, "Test123!");

    var adminResult = await userManager.CreateAsync(new AppUser
    {
        UserName = "Admin",
    }, "Admin123!");

    if (adminResult.Succeeded)
    {
        var admin = await userManager.FindByNameAsync("Admin");

        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt => opt.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:3000"));

app.UseHttpsRedirection();

app.UseAuthentication();   

app.UseAuthorization();

app.MapControllers();

app.Run();
