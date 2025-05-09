﻿using DemoWebAPI.Dtos;
using DemoWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AuthMaster> AuthMasters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    option => option.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<User>()
                .ToTable("UserMaster", "dbo")
                .HasKey(u => u.UserMasterId);

            modelBuilder.Entity<AuthMaster>()
                .ToTable("AuthMaster", "dbo")
                .HasKey(u => u.AuthMasterId);
        }
    }
}
