using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
{
	public void Configure(EntityTypeBuilder<Attachment> builder)
	{
		builder.HasKey(x=>x.Id);

		builder.HasOne<Visit>()
			.WithMany(x=>x.Attachments)
			.HasForeignKey(x=>x.VisitId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(x => x.Id)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();

		builder.Property(x => x.VisitId)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();

		builder.Property(x => x.Extension)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();

		builder.Property(x => x.Content)
			.HasConversion(x => x.Value, x => new(x))
			.HasMaxLength(FileContent.MaxSize)
			.IsRequired();

		builder.Property(x => x.Name)
			.HasConversion(x => x.Value, x => new(x))
			.IsRequired();
	}
}
