using Integration.DataLayer;
using Integration.DataLayer.Repositories.CategoryRepository;
using Integration.DataLayer.Repositories.ProductRepository;
using Integration.DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the concrete classes and the interfaces.
// builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();


// ApplicationDbContext registration.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // options.UseInMemoryDatabase("proba");
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.UseSqlServer(builder.Configuration
    //    .GetConnectionString("DefaultConnection"),
    //     sqlServerOptionsAction: sqlOptions =>
    //     {
    //         sqlOptions.MigrationsAssembly("IS6IntegrationAspNetCoreMVC");
    //     });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
