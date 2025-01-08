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
    public required DbSet<Inventory> Inventories { get; set; }
    public required DbSet<Token> Tokens { get; set; }

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
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductID);
        
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductID);
        

         // 添加索引配置
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email) // 为 Email 添加唯一索引
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name); // 为 Category 的 Name 添加普通索引

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name) // 为 Product 的 Name 添加索引
            .HasDatabaseName("IX_Products_Name"); // 可指定索引名称

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.CategoryID); // 为 CategoryID 添加索引

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber) // 为 OrderNumber 添加唯一索引
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
    }
}
