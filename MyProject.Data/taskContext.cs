using System;
using Microsoft.EntityFrameworkCore;

namespace MyProject.Data
{
    public class taskContext : DbContext
    {
        public taskContext(DbContextOptions<taskContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define schema.
            modelBuilder.HasDefaultSchema("public");

            // UserRole entity.
            modelBuilder.Entity<UserRole>().HasKey(
                nameof(UserRole.user_id),
                nameof(UserRole.role));
        }


        public DbSet<User> Users { get; set; }
  
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Project> Projects { get; set; }


    }
}
