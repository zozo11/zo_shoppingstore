using System;
using Microsoft.EntityFrameworkCore;
using ZoOnlineStore.Models;

namespace ZoOnlineStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Order> Orders { get; set; }
    public required DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public required DbSet<Token> Tokens { get; set; }
    public required DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentID);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserID);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderID);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductID);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductID);
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Inventory)
            .WithOne(i => i.Product)
            .HasForeignKey<Product>(p => p.InventoryID);

        //index
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email) // Email index
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name) 
            .HasDatabaseName("IX_Products_Name"); 

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.CategoryID); 

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.UserID);

        modelBuilder.Entity<OrderItem>()
            .HasIndex(oi => oi.OrderID);

        modelBuilder.Entity<OrderItem>()
            .HasIndex(oi => oi.ProductID); 

        modelBuilder.Entity<Inventory>()
            .HasIndex(i => i.ProductID);

        modelBuilder.Entity<Token>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(t => t.UserID);
        
        modelBuilder.Entity<Order>()
        .Property(o => o.TotalAmount)
        .HasPrecision(18, 2); // 18 位总长度，2 位小数
        
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)            // ProductImage has one Product
            .WithMany(p => p.Images)      // Product has many ProductImages
            .HasForeignKey(pi => pi.ProductID)   // Foreign key is ProductID
            .OnDelete(DeleteBehavior.Cascade);  // Optional: Define delete behavior
    }
}
