using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderUserPermissions : BuilderBase<RL_UserPermission>, IEntityTypeConfiguration<RL_UserPermission>
    {
        public void Configure(EntityTypeBuilder<RL_UserPermission> builder)
        {
            Base(builder);
            builder.ToTable("RL_UserPermissions");
            builder.HasIndex(up => new { up.UserId, up.PermissionId }).IsUnique();
        }
    }
}
