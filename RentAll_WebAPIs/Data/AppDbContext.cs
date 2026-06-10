using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAll_WebAPIs.Models;
using RentAll_WebAPIs.Helpers;

namespace RentAll_WebAPIs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()!.ToLower());
                foreach (var prop in entity.GetProperties())
                    prop.SetColumnName(prop.GetColumnName().ToLower());
            }
            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = 1,
                    Username = "owner1",
                    Email = "owner1@test.com",
                    PasswordHash =
                        PasswordHelper.HashPassword(
                            "123456"),
                    Role = "Owner",
                    CreatedAt =
                        new DateTime(2025,1,1)
                },

                new User
                {
                    Id = 2,
                    Username = "owner2",
                    Email = "owner2@test.com",
                    PasswordHash =
                        PasswordHelper.HashPassword(
                            "123456"),
                    Role = "Owner",
                    CreatedAt =
                        new DateTime(2025,1,1)
                },

                new User
                {
                    Id = 3,
                    Username = "owner3",
                    Email = "owner3@test.com",
                    PasswordHash =
                        PasswordHelper.HashPassword(
                            "123456"),
                    Role = "Owner",
                    CreatedAt = new DateTime(2025,1,1)
                },
                new User
                {
                    Id = 4,
                    Username = "owner4",
                    Email = "owner4@test.com",
                    PasswordHash =
                        PasswordHelper.HashPassword(
                            "123456"),
                    Role = "Owner",
                    CreatedAt = new DateTime(2025,1,1)
                },
                new User {
                    Id = 5,
                    Username = "MUhammad Faisal",
                    Email = "m.faisaltech244@gmail.com",
                    PasswordHash = PasswordHelper.HashPassword("232"),
                    Role = "Owner",
                    CreatedAt = new DateTime(2026,6,10)
                }
                
            );

        }
    }
}
