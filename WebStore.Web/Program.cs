using Microsoft.EntityFrameworkCore;
using WebStore.DAL.EF;
using WebStore.Model;
using Microsoft.AspNetCore.Identity;
using WebStore.Services.Interfaces;
using WebStore.Services.ConcreteServices;
using WebStore.Services.Configuration.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Pobranie connection stringa z appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Rejestracja DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Rejestracja Identity (zgodnie z konfiguracją TPH z zadania 4.3)
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Rejestracja AutoMappera (przeskanuje projekt w poszukiwaniu profili)
builder.Services.AddAutoMapper(typeof(MainProfile));

// Rejestracja serwisów (mówimy: "gdy ktoś prosi o IProductService, daj mu ProductService")
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
