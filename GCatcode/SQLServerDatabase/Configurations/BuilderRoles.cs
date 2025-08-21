using GCatcode.SQLServerDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.SQLServerDatabase.Configurations
{
    public class BuilderRoles : BuilderBase<Rol>, IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            Base(builder);
        }
    }
}