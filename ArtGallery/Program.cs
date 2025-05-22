using ArtGallery.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var conString = builder.Configuration.GetConnectionString("ArtGalleryDb") ??
     throw new InvalidOperationException("Connection string 'ArtGalleryDb'" +
    " not found.");
builder.Services.AddDbContext<GalleryDBContext>(options => options.UseSqlServer(conString));

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

