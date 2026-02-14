using GCatcode.DataBase.Configurations;
using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace GCatcode.DataBase
{
    public class AppDBContext : DbContext
    {        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuilderUsers());
            modelBuilder.ApplyConfiguration(new BuilderRoles());
            modelBuilder.ApplyConfiguration(new BuilderUserRoles());
            modelBuilder.ApplyConfiguration(new BuilderRefreshTokens());
            modelBuilder.ApplyConfiguration(new BuilderViews());
            modelBuilder.ApplyConfiguration(new BuilderViewRoles());
            
            // Configuraciones para RBAC + ABAC
            modelBuilder.ApplyConfiguration(new BuilderPermissions());
            modelBuilder.ApplyConfiguration(new BuilderRolePermissions());
            modelBuilder.ApplyConfiguration(new BuilderUserPermissions());
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {           
            
        }
    }
}