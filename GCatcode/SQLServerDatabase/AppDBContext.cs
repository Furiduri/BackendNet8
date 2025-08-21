using GCatcode.SQLServerDatabase.Configurations;
using GCatcode.SQLServerDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GCatcode.SQLServerDatabase
{
    public class AppDBContext : DbContext
    {
        public DbSet<Rol> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRol> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuilderUsers());
            modelBuilder.ApplyConfiguration(new BuilderRoles());
            modelBuilder.ApplyConfiguration(new BuilderUserRol());
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            
        }
    }
}