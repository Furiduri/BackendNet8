using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderRolePermissions : BuilderBase<RL_RolePermission>, IEntityTypeConfiguration<RL_RolePermission>
    {
        public void Configure(EntityTypeBuilder<RL_RolePermission> builder)
        {
            Base(builder);
            builder.ToTable("RL_RolePermissions");
            builder.HasIndex(rp => new { rp.RolId, rp.PermissionId }).IsUnique();

            // Asignación de permisos por rol
            builder.HasData([
                // Dev (RolId = 1) - Todos los permisos
                new RL_RolePermission { RolePermissionId = 1, RolId = 1, PermissionId = 1, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 2, RolId = 1, PermissionId = 2, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 3, RolId = 1, PermissionId = 3, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 4, RolId = 1, PermissionId = 4, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 5, RolId = 1, PermissionId = 5, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 6, RolId = 1, PermissionId = 6, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 7, RolId = 1, PermissionId = 7, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 8, RolId = 1, PermissionId = 8, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 9, RolId = 1, PermissionId = 9, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 10, RolId = 1, PermissionId = 10, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 11, RolId = 1, PermissionId = 11, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 12, RolId = 1, PermissionId = 12, CreateTime = seedDate, LastUpdated = seedDate },

                // Admin (RolId = 4) - Permisos administrativos
                new RL_RolePermission { RolePermissionId = 13, RolId = 4, PermissionId = 1, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 14, RolId = 4, PermissionId = 2, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 15, RolId = 4, PermissionId = 3, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 16, RolId = 4, PermissionId = 4, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 17, RolId = 4, PermissionId = 5, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 18, RolId = 4, PermissionId = 7, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 19, RolId = 4, PermissionId = 8, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 20, RolId = 4, PermissionId = 9, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 21, RolId = 4, PermissionId = 10, CreateTime = seedDate, LastUpdated = seedDate },

                // User (RolId = 3) - Solo lectura
                new RL_RolePermission { RolePermissionId = 22, RolId = 3, PermissionId = 1, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 23, RolId = 3, PermissionId = 4, CreateTime = seedDate, LastUpdated = seedDate },
                new RL_RolePermission { RolePermissionId = 24, RolId = 3, PermissionId = 9, CreateTime = seedDate, LastUpdated = seedDate },

                // Guest (RolId = 2) - Lectura muy limitada
                new RL_RolePermission { RolePermissionId = 25, RolId = 2, PermissionId = 9, CreateTime = seedDate, LastUpdated = seedDate }
            ]);
        }
    }
}
