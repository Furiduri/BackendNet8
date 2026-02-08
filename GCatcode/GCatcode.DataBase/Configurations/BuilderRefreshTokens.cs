using GCatcode.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCatcode.DataBase.Configurations
{
    public class BuilderRefreshTokens : BuilderBase<RefreshToken>, IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Configuración base heredada (Available, CreateTime, LastUpdated)
            Base(builder);

            // Configuración de la tabla
            builder.ToTable("RefreshTokens");

            // Configuración de la clave primaria
            builder.HasKey(rt => rt.RefreshTokenId);

            // Configuración de propiedades
            builder.Property(rt => rt.RefreshTokenId)
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.ExpiryDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(rt => rt.CreatedByIp)
                .HasMaxLength(50);

            builder.Property(rt => rt.RevokedAt)
                .HasColumnType("datetime2");

            builder.Property(rt => rt.RevokedByIp)
                .HasMaxLength(50);

            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(500);

            // Configuración de índices
            builder.HasIndex(rt => rt.Token)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token");

            builder.HasIndex(rt => rt.UserId)
                .HasDatabaseName("IX_RefreshTokens_UserId");

            builder.HasIndex(rt => new { rt.ExpiryDate, rt.IsRevoked })
                .HasDatabaseName("IX_RefreshTokens_ExpiryDate_IsRevoked");

            // Configuración de relación con User
            builder.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefreshTokens_Users");

            // Ignorar propiedades calculadas (no se mapean a la BD)
            builder.Ignore(rt => rt.IsExpired);
            builder.Ignore(rt => rt.IsActive);
        }
    }
}