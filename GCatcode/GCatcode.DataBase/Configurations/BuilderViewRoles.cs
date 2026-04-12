using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderViewRoles : BuilderBase<RL_ViewRol>, IEntityTypeConfiguration<RL_ViewRol>
    {
        public void Configure(EntityTypeBuilder<RL_ViewRol> builder)
        {
            Base(builder);
            builder.ToTable("RL_ViewRoles");     
            builder
                .HasKey(ur => new { ur.ViewId, ur.RolId });
            builder
                .HasOne(ur => ur.View)
                .WithMany(u => u.ViewRols)
                .HasForeignKey(ur => ur.ViewId);
            builder
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.ViewRols)
                .HasForeignKey(ur => ur.RolId);

            
            builder.HasData(
                // Dev (RolId = 0) tiene acceso a todas las vistas
                new RL_ViewRol { RolId = (int)RolesType.Dev, ViewId = 1, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Dashboard
                new RL_ViewRol { RolId = (int)RolesType.Dev, ViewId = 3, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Usuarios
                new RL_ViewRol { RolId = (int)RolesType.Dev, ViewId = 4, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Roles
                new RL_ViewRol { RolId = (int)RolesType.Dev, ViewId = 5, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Permisos

                // Admin(RolId = 3) tiene acceso a todas las vistas
                new RL_ViewRol { RolId = (int)RolesType.Admin, ViewId = 1, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Dashboard
                new RL_ViewRol { RolId = (int)RolesType.Admin, ViewId = 3, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Usuarios
                new RL_ViewRol { RolId = (int)RolesType.Admin, ViewId = 4, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Roles
                new RL_ViewRol { RolId = (int)RolesType.Admin, ViewId = 5, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Permisos
                                
                new RL_ViewRol {  RolId = (int)RolesType.User, ViewId = 1, CreateTime = seedDate, LastUpdated = seedDate, Available = true }, // Dashboard
                new RL_ViewRol { RolId = (int)RolesType.Guest, ViewId = 1, CreateTime = seedDate, LastUpdated = seedDate, Available = true } // Dashboard
            );
        }
    }
}