using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class ExternalUserLoginEntityConfiguration
    : IEntityTypeConfiguration<ExternalUserLogin>
{
    public void Configure(EntityTypeBuilder<ExternalUserLogin> builder)
    {

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasConversion(
            x => x.Value,
                    x => new(x))
            .IsRequired();

        builder.Property(x => x.ProvidedKey)
            .HasConversion(
    x => x.Value,
            x => new(x))
            .IsRequired();

        builder.HasOne<User>()
            .WithOne(u => u.ExternalUserProvider)
            .HasForeignKey<ExternalUserLogin>(uv => uv.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
