﻿using Integration.Models.Categories;
using Integration.Models.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integration.DataLayer;
public class ApplicationDbContext : IdentityDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Set the DB Tables
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }
}