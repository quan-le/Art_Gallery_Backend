using ArtGallery.Persistence;
using ArtGallery.Persistence.InterfaceDAO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

