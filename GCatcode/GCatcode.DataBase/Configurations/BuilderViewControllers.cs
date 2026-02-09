using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderViewControllers : IEntityTypeConfiguration<RL_ViewController>
    {
        public void Configure(EntityTypeBuilder<RL_ViewController> builder)
        {
            builder.ToTable("RL_ViewControllers");
            builder
                .HasKey(ur => new { ur.ViewId, ur.ControllerId });
            builder
                .HasOne(ur => ur.View)
                .WithMany(u => u.ViewControllers)
                .HasForeignKey(ur => ur.ViewId);
            builder
                .HasOne(ur => ur.Controller)
                .WithMany(r => r.ViewControllers)
                .HasForeignKey(ur => ur.ControllerId);

            // Seed: Relación Vistas -> Controladores
            builder.HasData(                
                new RL_ViewController { ViewId = 3, ControllerId = 1 }, // Users View -> UsersController
                new RL_ViewController { ViewId = 4, ControllerId = 2 }, // Roles View -> RolesController
                new RL_ViewController { ViewId = 5, ControllerId = 3 }  // Permisos View -> PermissionsController 
            );
        }
    }
}