using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderUserRol : BuilderBase<UserRol>, IEntityTypeConfiguration<UserRol>
    {
        public void Configure(EntityTypeBuilder<UserRol> builder)
        {
            Base(builder);
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
        }
    }
}