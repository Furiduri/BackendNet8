using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.SQLServerDatabase.Configurations
{
    public class BuilderBase<T> where T : BaseModel
    {
        public void Base(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.CreateTime)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.LastUpdated)
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.Available)
                .HasDefaultValue(true);
        }
    }
}