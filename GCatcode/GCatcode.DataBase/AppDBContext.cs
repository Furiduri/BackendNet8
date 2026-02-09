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
            modelBuilder.ApplyConfiguration(new BuilderControllers());
            modelBuilder.ApplyConfiguration(new BuilderControllerMethods());
            modelBuilder.ApplyConfiguration(new BuilderViews());
            modelBuilder.ApplyConfiguration(new BuilderViewRoles());
            modelBuilder.ApplyConfiguration(new BuilderViewControllers());            
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {           
            
        }
    }
}