﻿using KingsCut.Web.Data.Entities;
using KingsCut.Web.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<KingsCutRole> KingsCutRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RoleService> RoleServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Configurekeys(modelBuilder);
            ConfigureIndexes(modelBuilder);
     
            base.OnModelCreating(modelBuilder);


        }

        private void Configurekeys(ModelBuilder builder)
        {
            //Role Permissions
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                                    .WithMany(r => r.RolePermissions)
                                                    .HasForeignKey(rp => rp.RoleId);                                                    

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                                    .WithMany(p => p.RolePermissions)
                                                    .HasForeignKey(rp => rp.PermissionId);

            //Role Services
            builder.Entity<RoleService>().HasKey(rs => new { rs.RoleId, rs.ServiceId });
            builder.Entity<RoleService>().HasOne(rs => rs.Role)
                                                    .WithMany(r => r.RoleServices)
                                                    .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RoleService>().HasOne(rs => rs.Service)
                                                    .WithMany(p => p.RoleServices)
                                                    .HasForeignKey(rs => rs.ServiceId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            //Roles
            builder.Entity<KingsCutRole>().HasIndex(r => r.Name).IsUnique();

            //Users
            builder.Entity<User>().HasIndex(u => u.Document).IsUnique();

            //Products
            builder.Entity<Product>().HasIndex(p => p.Name).IsUnique();

            //Services
            builder.Entity<Service>().HasIndex(s => s.Name).IsUnique();

        }
    }
}
