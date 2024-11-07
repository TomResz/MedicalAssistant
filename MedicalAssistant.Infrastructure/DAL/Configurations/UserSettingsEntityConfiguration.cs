using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class UserSettingsEntityConfiguration 
	: IEntityTypeConfiguration<UserSettings>
{
	public void Configure(EntityTypeBuilder<UserSettings> builder)
	{
		builder.HasOne(x => x.User)
			 .WithOne(x => x.UserSettings)
			 .HasForeignKey<UserSettings>(x => x.UserId) 
			 .OnDelete(DeleteBehavior.Cascade);

		builder.HasKey(x => x.UserId);

		builder.Property(x => x.UserId)
			.HasConversion(x=>x.Value,
				x=> new(x))
			.IsRequired();

		builder.Property(x => x.NotificationLanguage)
			.HasConversion(x => x.ToString(),
			x => NotificationLanguage.FromString(x)
			);
	}
}
