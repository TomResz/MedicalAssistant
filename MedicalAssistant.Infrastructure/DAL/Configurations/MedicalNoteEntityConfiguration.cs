using MedicalAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class MedicalNoteEntityConfiguration : IEntityTypeConfiguration<MedicalNote>
{
    public void Configure(EntityTypeBuilder<MedicalNote> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.MedicalNotes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.HasQueryFilter(x => x.User.IsActive);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Note)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.HasIndex(b => new
            {
                b.Note
            })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");
    }
}