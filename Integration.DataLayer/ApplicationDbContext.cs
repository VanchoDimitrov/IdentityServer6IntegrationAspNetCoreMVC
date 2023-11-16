using Integration.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integration.DataLayer;
public class ApplicationDbContext : IdentityDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Set the DB Tables
    public DbSet<Category> categories { get; set; }

    public DbSet<Product> products { get; set; }
}