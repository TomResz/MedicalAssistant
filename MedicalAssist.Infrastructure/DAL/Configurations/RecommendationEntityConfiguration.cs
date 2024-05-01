using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssist.Infrastructure.DAL.Configurations;
internal sealed class RecommendationEntityConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.HasKey(x=> x.Id);

        builder.Property(x=> x.Id)
            .HasConversion(x=> x.Value, x=> new RecommendationId(x))
            .IsRequired();

        builder.Property(x => x.ExtraNote)
            .HasConversion(x => x.Value, x => new Note(x))
            .HasMaxLength(500)
            .IsRequired(false);
           
        builder.Property(x=> x.CreatedAt)
            .HasConversion(x=> x.Value, x => new Date(x))
            .IsRequired();

        builder.ComplexProperty(x => x.Medicine, conf =>
        {
            conf.Property(x => x.Name)
                .HasConversion(x => x.Value, x => new MedicineName(x))
                .IsRequired();

            conf.Property(x => x.Quantity)
                .HasConversion(x => x.Value, x => new Quantity(x))
                .IsRequired();
        });
    }
}
