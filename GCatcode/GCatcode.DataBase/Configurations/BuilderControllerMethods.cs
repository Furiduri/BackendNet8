using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderControllerMethods : BuilderBase<CL_ControllerMethod>, IEntityTypeConfiguration<CL_ControllerMethod>
    {
        public void Configure(EntityTypeBuilder<CL_ControllerMethod> builder)
        {
            Base(builder);
            builder.ToTable("CL_ControllerMethods");
            builder.HasIndex(m => new { m.ControllerId, m.Name }).IsUnique();
            
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                // Controlador Users (ControllerId = 1)
                new CL_ControllerMethod { 
                    MethodId = 1, 
                    ControllerId = 1, 
                    Name = "GetAll", 
                    HttpMethod = "GET", 
                    Endpoint = "/admin/Users", 
                    Description = "Listar usuarios", 
                    PermissionType = PermissionType.Read,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 2, 
                    ControllerId = 1, 
                    Name = "GetById", 
                    HttpMethod = "GET", 
                    Endpoint = "/admin/Users/{id}", 
                    Description = "Obtener usuario por ID", 
                    PermissionType = PermissionType.Read,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 3, 
                    ControllerId = 1, 
                    Name = "Create", 
                    HttpMethod = "POST", 
                    Endpoint = "/admin/Users", 
                    Description = "Crear usuario", 
                    PermissionType = PermissionType.Write,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 4, 
                    ControllerId = 1, 
                    Name = "Update", 
                    HttpMethod = "PUT", 
                    Endpoint = "/admin/Users/{id}", 
                    Description = "Actualizar usuario", 
                    PermissionType = PermissionType.Write,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 5, 
                    ControllerId = 1, 
                    Name = "Delete", 
                    HttpMethod = "DELETE", 
                    Endpoint = "/admin/Users/{id}", 
                    Description = "Eliminar usuario", 
                    PermissionType = PermissionType.ReadWrite,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },

                // Controlador Roles (ControllerId = 2)
                new CL_ControllerMethod { 
                    MethodId = 6, 
                    ControllerId = 2, 
                    Name = "GetAll", 
                    HttpMethod = "GET", 
                    Endpoint = "/admin/Roles", 
                    Description = "Listar roles", 
                    PermissionType = PermissionType.Read,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 7, 
                    ControllerId = 2, 
                    Name = "GetById", 
                    HttpMethod = "GET", 
                    Endpoint = "/admin/Roles/{id}", 
                    Description = "Obtener rol por ID", 
                    PermissionType = PermissionType.Read,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 8, 
                    ControllerId = 2, 
                    Name = "Create", 
                    HttpMethod = "POST", 
                    Endpoint = "/admin/Roles", 
                    Description = "Crear rol", 
                    PermissionType = PermissionType.Write,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 9, 
                    ControllerId = 2, 
                    Name = "Update", 
                    HttpMethod = "PUT", 
                    Endpoint = "/admin/Roles/{id}", 
                    Description = "Actualizar rol", 
                    PermissionType = PermissionType.Write,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_ControllerMethod { 
                    MethodId = 10, 
                    ControllerId = 2, 
                    Name = "Delete", 
                    HttpMethod = "DELETE", 
                    Endpoint = "/admin/Roles/{id}", 
                    Description = "Eliminar rol", 
                    PermissionType = PermissionType.ReadWrite,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                }
            );
        }
    }
}