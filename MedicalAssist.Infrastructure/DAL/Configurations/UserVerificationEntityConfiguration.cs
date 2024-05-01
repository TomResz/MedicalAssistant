using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssist.Infrastructure.DAL.Configurations;
internal sealed class UserVerificationEntityConfiguration
	: IEntityTypeConfiguration<UserVerification>
{
	public void Configure(EntityTypeBuilder<UserVerification> builder)
	{
		builder.HasKey(x=> x.Id);



		builder.Property(x => x.Id)
			.HasConversion(x => x.Value, x => new UserVerificationId(x))
			.IsRequired();

		builder.Property(x => x.UserId)
			.HasConversion(x => x.Value, value => new UserId(value))
			.IsRequired();

		builder.Property(x => x.CodeHash)
			.HasConversion(x => x.Value, value => new CodeHash(value))
			.IsRequired();
		
		builder.Property(x=> x.ExpirationDate)
			.HasConversion(x=> x.Value, value => new Date(value))
			.IsRequired();


		builder.HasOne<User>()
			.WithOne(u => u.UserVerification)
			.HasForeignKey<UserVerification>(uv => uv.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
