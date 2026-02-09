using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderRoles : BuilderBase<CL_Rol>, IEntityTypeConfiguration<CL_Rol>
    {
        public void Configure(EntityTypeBuilder<CL_Rol> builder)
        {
            Base(builder);
            builder.ToTable("CL_Roles");
            
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData([
                new CL_Rol
                {
                    RolId = (int)RolesType.Dev,
                    Name = RolesType.Dev.ToString(),
                    Description = "All Access Development",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_Rol
                {
                    RolId = (int)RolesType.Guest,
                    Name = RolesType.Guest.ToString(),
                    Description = "Limited Access",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_Rol
                {
                    RolId = (int)RolesType.User,
                    Name = RolesType.User.ToString(),
                    Description = "Access parcial",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_Rol
                {
                    RolId = (int)RolesType.Admin,
                    Name = RolesType.Admin.ToString(),
                    Description = "All Access Administration",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                }
            ]);
        }
    }
}