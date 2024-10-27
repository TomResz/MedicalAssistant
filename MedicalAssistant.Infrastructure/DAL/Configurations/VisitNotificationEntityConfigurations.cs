using MedicalAssistant.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class VisitNotificationEntityConfigurations : IEntityTypeConfiguration<VisitNotification>
{
	public void Configure(EntityTypeBuilder<VisitNotification> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne<Visit>()
			.WithMany(x => x.Notifications)
			.HasForeignKey(x => x.VisitId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(x => x.ScheduledDateUtc)
			.HasConversion(x=>x.Value,
				x=>new(x))
			.IsRequired();

		builder.Property(x => x.Id)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();

		builder.Property(x => x.UserId)
			.HasConversion(
			x => x.Value,
			   x => new(x))
			.IsRequired();
		builder.Property(x => x.VisitId)
			.HasConversion(
			x => x.Value,
			   x => new(x))
			.IsRequired();
		builder.Property(x=>x.JobId)
			.IsRequired();
	}
}
