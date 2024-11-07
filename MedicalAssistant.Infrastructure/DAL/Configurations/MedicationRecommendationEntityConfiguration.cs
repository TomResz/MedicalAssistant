using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class MedicationRecommendationEntityConfiguration : IEntityTypeConfiguration<MedicationRecommendation>
{
    public void Configure(EntityTypeBuilder<MedicationRecommendation> builder)
    {
        builder.HasKey(x=> x.Id);
        builder.ToTable(nameof(MedicationRecommendation));

        builder.Property(x => x.VisitId)
            .HasConversion(
                x => x.Value,
                x => new(x))
            .IsRequired(false);

		builder.HasMany(x=>x.Notifications)
            .WithOne()
        	.HasForeignKey(x => x.MedicationRecommendationId)
        	.OnDelete(DeleteBehavior.Cascade);


		builder.HasOne(x=>x.Visit)
            .WithMany(x=>x.Recommendations)
            .HasForeignKey(x => x.VisitId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany(x=>x.MedicationRecommendations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x=> x.Id)
            .HasConversion(x=> x.Value, x=> new MedicationRecommendationId(x))
            .IsRequired();

        builder.Property(x => x.ExtraNote)
            .HasConversion(x => x.Value, x => new Note(x))
            .HasMaxLength(500)
            .IsRequired(false);
           
        builder.Property(x=> x.CreatedAt)
            .HasConversion(x=> x.Value, x => new Date(x))
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasConversion(x => x.Value, x => new Date(x))
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasConversion(x => x.Value, x => new Date(x))
            .IsRequired();

        builder.ComplexProperty(x => x.Medicine, conf =>
        {
            conf.Property(x => x.TimeOfDay)
                .HasConversion(x => x.Value, x => new TimeOfDay(x))
                .IsRequired();

            conf.Property(x => x.Name)
                .HasConversion(x => x.Value, x => new MedicineName(x))
                .IsRequired();

            conf.Property(x => x.Quantity)
                .HasConversion(x => x.Value, x => new Quantity(x))
                .IsRequired();
        });
    }
}
