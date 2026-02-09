using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderUsers : BuilderBase<TR_User>, IEntityTypeConfiguration<TR_User>
    {
        public void Configure(EntityTypeBuilder<TR_User> builder)
        {
            Base(builder);
            builder.ToTable("TR_Users");
            
            // Fecha fija para el seed inicial
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData([
                new TR_User { 
                    UserId = 1, 
                    UserName = "Dev", 
                    Email = "dev@local.com", 
                    Password = Utils.Argon2Helper.HashPassword("Dev12345"),
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new TR_User { 
                    UserId = 2, 
                    UserName = "Guest", 
                    Email = "guest@local.com", 
                    Password = Utils.Argon2Helper.HashPassword("Gest12345"),
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new TR_User { 
                    UserId = 3, 
                    UserName = "User", 
                    Email = "user@local.com", 
                    Password = Utils.Argon2Helper.HashPassword("User12345"),
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new TR_User { 
                    UserId = 4, 
                    UserName = "Admin", 
                    Email = "admin@local.com", 
                    Password = Utils.Argon2Helper.HashPassword("Admin12345"),
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                }
            ]);
        }
    }
}