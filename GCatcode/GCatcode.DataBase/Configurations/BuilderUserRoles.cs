using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderUserRoles : IEntityTypeConfiguration<RL_UserRol>
    {
        public void Configure(EntityTypeBuilder<RL_UserRol> builder)
        {
            builder.ToTable("RL_UserRoles");
            builder
                .HasKey(ur => new { ur.UserId, ur.RolId });
            builder
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            builder
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RolId);

            builder.HasData([
                new RL_UserRol { UserId = 1, RolId = (int)RolesType.Dev },
                new RL_UserRol { UserId = 1, RolId = (int)RolesType.Guest },
                new RL_UserRol { UserId = 1, RolId = (int)RolesType.User },
                new RL_UserRol { UserId = 1, RolId = (int)RolesType.Admin },
                new RL_UserRol { UserId = 2, RolId = (int)RolesType.Guest },
                new RL_UserRol { UserId = 3, RolId = (int)RolesType.User },
                new RL_UserRol { UserId = 4, RolId = (int)RolesType.Admin }
            ]);
        }
    }
}