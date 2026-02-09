using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderControllers : BuilderBase<CL_Controller>, IEntityTypeConfiguration<CL_Controller>
    {
        public void Configure(EntityTypeBuilder<CL_Controller> builder)
        {
            Base(builder);
            builder.ToTable("CL_Controllers");
            
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData([
                new CL_Controller
                {
                    ControllerId = 1,
                    Name = "UsersController",
                    Description = "Controller for users management",
                    ControllerPath = "admin/users",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_Controller
                {
                    ControllerId = 2,
                    Name = "RolesController",
                    Description = "Controller for roles management",
                    ControllerPath = "admin/roles",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                },
                new CL_Controller
                {
                    ControllerId = 3,
                    Name = "PermissionsController",
                    Description = "Controller for permissions management",
                    ControllerPath = "admin/permissions",
                    CreateTime = seedDate,
                    LastUpdated = seedDate,
                    Available = true
                }
            ]);
        }
    }
}