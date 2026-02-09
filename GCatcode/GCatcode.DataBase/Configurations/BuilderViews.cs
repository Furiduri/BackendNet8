using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderViews : BuilderBase<CL_View>, IEntityTypeConfiguration<CL_View>
    {
        public void Configure(EntityTypeBuilder<CL_View> builder)
        {
            Base(builder);
            builder.ToTable("CL_Views");
            
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData([
                new CL_View { 
                    ViewId = 1, 
                    Name = "Dashboard", 
                    Route = "/dashboard", 
                    Icon = "DashboardIcon", 
                    Order = 1, 
                    IsActive = true,
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_View
                {
                    ViewId = 2,
                    Name = "Admin",
                    Description = "View for Adim tools",
                    Route = "admin",
                    Icon = "admin",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true,
                    IsActive = true
                },
                new CL_View
                {
                    ViewId = 3,
                    Name = "Users",
                    Description = "View for users management",
                    Route = "admin/users",
                    ParentViewId = 1,
                    Icon = "user",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true,
                    IsActive = true
                },
                new CL_View
                {
                    ViewId = 4,
                    Name = "Roles",
                    Description = "View for roles management",
                    Route = "admin/roles",
                    ParentViewId = 1,
                    Icon = "list",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true,
                    IsActive = true
                },
                new CL_View
                {
                    ViewId = 5,
                    Name = "Permisos",
                    Description = "View for permisos management",
                    Route = "admin/permisos",
                    ParentViewId = 1,
                    Icon = "auth",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true,
                    IsActive = true
                }
            ]);
        }
    }
}