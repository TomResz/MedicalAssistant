using MedicalAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class RecommendationUsageEntityConfiguration : IEntityTypeConfiguration<RecommendationUsage>
{
	public void Configure(EntityTypeBuilder<RecommendationUsage> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x=>x.Id)
			.HasConversion(x=> x.Value, x=> new(x))
			.IsRequired();


		builder.Property(x => x.Date)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();

		builder.Property(x => x.TimeOfDay)
				.HasMaxLength(15)
				.IsRequired();

		builder.Property(x => x.MedicationRecommendationId)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();

		builder.HasOne(x => x.MedicationRecommendation)
			.WithMany(x => x.RecommendationUsages)
			.HasForeignKey(x => x.MedicationRecommendationId);


	}
}
