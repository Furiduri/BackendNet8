using GCatcode.SQLServerDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GCatcode.SQLServerDatabase
{
    public class AppDBContext : DbContext
    {
        public DbSet<Rol> Roles {  get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) 
            : base(options)
        {           
            
        }
    }
}
