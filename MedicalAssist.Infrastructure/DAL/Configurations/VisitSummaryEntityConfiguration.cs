using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssist.Infrastructure.DAL.Configurations;
internal sealed class VisitSummaryEntityConfiguration : IEntityTypeConfiguration<VisitSummary>
{
    public void Configure(EntityTypeBuilder<VisitSummary> builder)
    {
        builder
            .HasOne<Visit>()
            .WithOne()
            .HasForeignKey<VisitSummary>(x => x.VisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(x=> x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value, 
            value => new VisitSummaryId(value))
            .IsRequired();

        builder.Property(x => x.VisitId)
            .HasConversion
            (x => x.Value,
            x => new VisitId(x))
            .IsRequired();

        builder.Property(x => x.AddedDateUtc)
            .HasConversion(
            x => x.Value, 
            x => new Date(x))
            .IsRequired();
    }
}
