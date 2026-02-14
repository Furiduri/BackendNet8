using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderPermissions : BuilderBase<CL_Permission>, IEntityTypeConfiguration<CL_Permission>
    {
        public void Configure(EntityTypeBuilder<CL_Permission> builder)
        {
            Base(builder);
            builder.ToTable("CL_Permissions");
            builder.HasIndex(p => p.Code).IsUnique();
            builder.HasIndex(p => new { p.Resource, p.Action });

            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed de permisos base del sistema
            builder.HasData([
                // Users
                new CL_Permission { PermissionId = 1, Code = "users.read", Resource = "users", Action = "read", Description = "Ver usuarios", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 2, Code = "users.write", Resource = "users", Action = "write", Description = "Crear/Editar usuarios", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 3, Code = "users.delete", Resource = "users", Action = "delete", Description = "Eliminar usuarios", CreateTime = seedDate, LastUpdated = seedDate },
                
                // Roles
                new CL_Permission { PermissionId = 4, Code = "roles.read", Resource = "roles", Action = "read", Description = "Ver roles", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 5, Code = "roles.write", Resource = "roles", Action = "write", Description = "Crear/Editar roles", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 6, Code = "roles.delete", Resource = "roles", Action = "delete", Description = "Eliminar roles", CreateTime = seedDate, LastUpdated = seedDate },
                
                // Permissions
                new CL_Permission { PermissionId = 7, Code = "permissions.read", Resource = "permissions", Action = "read", Description = "Ver permisos", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 8, Code = "permissions.write", Resource = "permissions", Action = "write", Description = "Asignar permisos", CreateTime = seedDate, LastUpdated = seedDate },
                
                // Views
                new CL_Permission { PermissionId = 9, Code = "views.read", Resource = "views", Action = "read", Description = "Ver vistas/menús", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 10, Code = "views.write", Resource = "views", Action = "write", Description = "Gestionar vistas/menús", CreateTime = seedDate, LastUpdated = seedDate },
                
                // System
                new CL_Permission { PermissionId = 11, Code = "system.admin", Resource = "system", Action = "admin", Description = "Administración total del sistema", CreateTime = seedDate, LastUpdated = seedDate },
                new CL_Permission { PermissionId = 12, Code = "system.config", Resource = "system", Action = "config", Description = "Configurar sistema", CreateTime = seedDate, LastUpdated = seedDate }
            ]);
        }
    }
}
