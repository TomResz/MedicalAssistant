using MedicalAssistant.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class MedicationRecommendationNotificationEntityConfiguration
	: IEntityTypeConfiguration<MedicationRecommendationNotification>
{
	public void Configure(EntityTypeBuilder<MedicationRecommendationNotification> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne<MedicationRecommendation>()
			.WithMany(x=>x.Notifications)
			.HasForeignKey(x=>x.MedicationRecommendationId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(x => x.Id)
			.HasConversion(x => x.Value,
			x => new(x))
			.IsRequired();

		builder.Property(x => x.TriggerTimeUtc)
			.HasConversion(
				v => v.ToTimeSpan(),
				v => TimeOnly.FromTimeSpan(v)
			).HasColumnType("TIME");

		builder.Property(x=>x.Start)
			.HasConversion(x=>x.Value,
				x=>new(x))
			.IsRequired();

		builder.Property(x => x.End)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();
		
		builder.Property(x=>x.JobId)
			.IsRequired();
	}
}
