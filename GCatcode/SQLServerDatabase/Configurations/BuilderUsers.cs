using GCatcode.SQLServerDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.SQLServerDatabase.Configurations
{
    public class BuilderUsers : BuilderBase<User>, IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            Base(builder);
        }
    }
}