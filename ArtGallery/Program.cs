using ArtGallery.Persistence;
using ArtGallery.Persistence.InterfaceDAO;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Authorization services

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}",
            ValidateLifetime = true,
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("permissions", "write"));
    options.AddPolicy("User", policy => policy.RequireClaim("permissions", "read"));

});
//builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

//Dependency Injection
builder.Services.AddScoped<IArtifactDAO, ArtifactDAO>();
builder.Services.AddScoped<IArtistDAO, ArtistDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

//Get connection string from appsettings.json
var conString = builder.Configuration.GetConnectionString("ArtGalleryDb") ??
     throw new InvalidOperationException("Connection string 'ArtGalleryDb'" +
    " not found.");
builder.Services.AddDbContext<GalleryDBContext>(options => options.UseSqlServer(conString));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.Run();

