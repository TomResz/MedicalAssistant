using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class MedicalHistoryEntityConfiguration
    : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.HasKey(x=>x.Id);

        builder.HasOne<User>()
            .WithMany(x => x.MedicalHistories)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
        
        builder.HasOne(x=>x.Visit)
            .WithMany(x=>x.MedicalHistories)
            .HasForeignKey(x=>x.VisitId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasMany(x => x.DiseaseStages)
            .WithOne()
            .HasForeignKey(x => x.MedicalHistoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.Property(x=>x.Id)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .IsRequired();

        builder.Property(x => x.DiseaseStartDate)
            .HasConversion(x => x.Value,
                x => new(x))
            .IsRequired();
        
        builder.Property(x => x.DiseaseEndDate)
            .HasConversion(x => x.Value,
                x => new(x))
            .IsRequired(false);
        
        builder.Property(x=>x.UserId)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .IsRequired();
        
        builder.Property(x=>x.VisitId)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .IsRequired(false);
        
        
        builder.Property(x=>x.DiseaseName)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .HasMaxLength(DiseaseName.MaxLenght)
            .IsRequired();

        builder.Property(x=>x.Notes)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .IsRequired(false);

        builder.Property(x=>x.SymptomDescription)
            .HasConversion(x=>x.Value,
                x=> new(x))
            .IsRequired(false);
    }
}