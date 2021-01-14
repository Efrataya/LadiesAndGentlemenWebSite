using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LadiesAndGentlemenWebSite.Models;

namespace LadiesAndGentlemenWebSite.Data
{
    public class LadiesAndGentlemenWebSiteContext : DbContext
    {
        public LadiesAndGentlemenWebSiteContext (DbContextOptions<LadiesAndGentlemenWebSiteContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasKey(t => new { t.ProductId, t.OrderId });

            modelBuilder.Entity<Cart>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<Cart>()
                .HasOne(pt => pt.Order)
                .WithMany(t => t.Carts)
                .HasForeignKey(pt => pt.OrderId);
        }
        public DbSet<LadiesAndGentlemenWebSite.Models.Address> Address { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.Cart> Cart { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.Category> Category { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.Client> Client { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.Order> Order { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.Product> Product { get; set; }
        public DbSet<LadiesAndGentlemenWebSite.Models.SubCategory> SubCategory { get; set; }
    }
}
