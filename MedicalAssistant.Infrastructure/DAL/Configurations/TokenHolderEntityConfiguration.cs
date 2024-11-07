using MedicalAssistant.Domain.ComplexTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class TokenHolderEntityConfiguration : IEntityTypeConfiguration<TokenHolder>
{
	public void Configure(EntityTypeBuilder<TokenHolder> builder)
	{
		builder.HasKey(x=>x.Id);

		builder.Property(x=>x.Id)
			.HasConversion(x=> x.Value,
				x=> new(x))
			.IsRequired();

		builder.Property(x => x.UserId)
			.HasConversion(x => x.Value, x => new Domain.ValueObjects.IDs.UserId(x))
			.IsRequired();
		
		builder.Property(x => x.RefreshToken)
			.HasColumnName(nameof(TokenHolder.RefreshToken))
			.HasConversion(x => x!.Value, x => new(x))
			.IsRequired(false);

		builder.Property(x => x.RefreshTokenExpirationUtc)
			.HasColumnName(nameof(TokenHolder.RefreshTokenExpirationUtc))
			.HasConversion(x => x!.Value, x => new(x))
			.IsRequired(false);

		builder.HasOne<Domain.Entities.User>()
			.WithMany(x=>x.RefreshTokens)
			.HasForeignKey(x=>x.UserId)
			.OnDelete(DeleteBehavior.Cascade);

	}
}
