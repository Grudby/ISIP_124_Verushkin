using System;
using System.IO;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Data
{
    public class MarketplaceContext : DbContext
    {
        public const string DatabaseFileName = "MarketplaceDatabase.db";

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<PickupPoint> PickupPoints => Set<PickupPoint>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
            optionsBuilder.UseSqlite($"Data Source={path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(user => user.Login).HasMaxLength(32).IsRequired();
                entity.HasIndex(user => user.Login).IsUnique();
                entity.Property(user => user.PasswordHash).IsRequired();
                entity.Property(user => user.PasswordSalt).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.Property(category => category.Name).HasMaxLength(64).IsRequired();
                entity.HasIndex(category => category.Name).IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.Property(product => product.Name).HasMaxLength(128).IsRequired();
                entity.Property(product => product.Description).HasMaxLength(512);
                entity.Property(product => product.Price).HasColumnType("decimal(10,2)");
                entity.HasOne(product => product.Category)
                      .WithMany(category => category.Products)
                      .HasForeignKey(product => product.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItem");
                entity.HasIndex(item => new { item.UserId, item.ProductId }).IsUnique();
                entity.HasOne(item => item.User)
                      .WithMany(user => user.CartItems)
                      .HasForeignKey(item => item.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(item => item.Product)
                      .WithMany()
                      .HasForeignKey(item => item.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PickupPoint>(entity =>
            {
                entity.ToTable("PickupPoint");
                entity.Property(point => point.Address).HasMaxLength(256).IsRequired();
                entity.Property(point => point.WorkingHours).HasMaxLength(64).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasOne(order => order.User)
                      .WithMany(user => user.Orders)
                      .HasForeignKey(order => order.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(order => order.PickupPoint)
                      .WithMany()
                      .HasForeignKey(order => order.PickupPointId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.ToTable("OrderProduct");
                entity.Property(item => item.Price).HasColumnType("decimal(10,2)");
                entity.HasOne(item => item.Order)
                      .WithMany(order => order.OrderProducts)
                      .HasForeignKey(item => item.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(item => item.Product)
                      .WithMany()
                      .HasForeignKey(item => item.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
