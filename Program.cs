using MicroBlog.Models;
using MicroBlog.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IDataRepository<>), typeof(AuthorRepository<>));
builder.Services.AddScoped(typeof(IDataRepository<>), typeof(BlogRepository<>));
builder.Services.AddScoped(typeof(IDataContext), typeof(DataContext));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action}/{id?}");

app.MapFallbackToFile("index.html");
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.Run();
