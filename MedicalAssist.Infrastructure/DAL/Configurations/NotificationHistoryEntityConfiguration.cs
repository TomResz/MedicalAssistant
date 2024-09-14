using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssist.Infrastructure.DAL.Configurations;
internal sealed class NotificationHistoryEntityConfiguration
	: IEntityTypeConfiguration<NotificationHistory>
{
	public void Configure(EntityTypeBuilder<NotificationHistory> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();
		builder.Property(x => x.UserId).HasColumnName("UserId");

		builder.Property(x => x.UserId)
			.HasConversion(x => x.Value, x => new UserId(x))
			.IsRequired();

		builder.HasOne<User>()
			.WithMany(x=>x.NotificationHistories)
			.HasForeignKey(x=>x.UserId)
			.OnDelete(DeleteBehavior.Cascade)
			.IsRequired();


		builder.Property(x => x.ContentJson)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();

		builder.Property(x => x.Type)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();

		builder.Property(x => x.PublishedDate)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired();

		builder.Property(x => x.DateOfRead)
			.HasConversion(x => x.Value,
				x => new(x))
			.IsRequired(false);
	}
}
