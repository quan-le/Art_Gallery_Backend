using ArtGallery.Persistence;
using ArtGallery.Persistence.InterfaceDAO;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using Scalar.AspNetCore;
using ArtGallery.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

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
    options.AddPolicy("Admin", policy => policy.RequireClaim("permissions", "write", "read"));
    options.AddPolicy("User", policy => policy.RequireClaim("permissions", "read"));

});

//---Add Swagger Services
builder.Services.AddSwaggerService();
//Dependency Injection of Model
builder.Services.AddScoped<IArtifactDAO, ArtifactDAO>();
builder.Services.AddScoped<IArtistDAO, ArtistDAO>();
//builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<ITagDAO, TagDAO>();

//Dependency Injecction of AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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

app.UseStaticFiles();

//---enable Swagger
app.UseSwagger();                                       
app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});


app.MapScalarApiReference(options =>
{
    options.Title = "Art Gallery API";
    options.DarkMode = true;
    options.ShowSidebar = true;
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    //Prefill Authentication(Actual authentication implementation is done with swagger)
    options.Authentication = new ScalarAuthenticationOptions();
    options.AddPreferredSecuritySchemes("OAuth2", "BearerAuth");
    options.AddOAuth2Authentication("OAuth2", scheme =>
    {
        scheme.Flows = new ScalarFlows
        {
            AuthorizationCode = new AuthorizationCodeFlow
            {
                ClientId = builder.Configuration["Auth0:SwaggerClientId"],
                ClientSecret = $"{builder.Configuration["Auth0:SwaggerClientSecret"]}",
                AuthorizationUrl = $"https://{builder.Configuration["Auth0:Domain"]}/authorize?audience={builder.Configuration["Auth0:Audience"]}",
                TokenUrl = $"https://{builder.Configuration["Auth0:Domain"]}/oauth/token",
                //RedirectUri = $"https://localhost:7291/scalar"
                
            }
        };
        scheme.DefaultScopes = [ "read", "write" ];
        scheme.Description = "OAuth2 Authentication by Auth0 for Art Gallery API";
    });
    //options.WithPersistentAuthentication();
}).AllowAnonymous();

app.UseSwaggerUI(setup =>
{
    setup.InjectStylesheet("/styles/theme-dark-high-constrast.css");
    
    setup.OAuthClientId(builder.Configuration["Auth0:SwaggerClientId"]);
    setup.OAuthClientSecret(builder.Configuration["Auth0:SwaggerClientSecret"]);
    setup.OAuthUsePkce();
    setup.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
    {
        { "audience", $"{builder.Configuration["Auth0:Audience"]}" }
    });
    
});

app.Run();

