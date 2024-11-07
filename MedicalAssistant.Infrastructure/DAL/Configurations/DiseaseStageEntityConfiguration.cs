using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class DiseaseStageEntityConfiguration
    : IEntityTypeConfiguration<DiseaseStage>
{
    public void Configure(EntityTypeBuilder<DiseaseStage> builder)
    {
        builder.HasKey(x=>x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value,
                x => new(x))
            .IsRequired();
        
        builder.HasOne<Visit>()
            .WithMany(x=>x.DiseaseStages)
            .HasForeignKey(x=>x.VisitId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false); 
        
        builder.HasOne<MedicalHistory>()
            .WithMany(x=>x.DiseaseStages)
            .HasForeignKey(x=>x.MedicalHistoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

		builder.Property(x => x.MedicalHistoryId)
	        .HasConversion(x => x.Value,
	        	x => new(x))
	        .IsRequired();


		builder.Property(x => x.Name)
            .HasConversion(x => x.Value,
                x => new(x))
            .HasMaxLength(DiseaseStageName.MaxNameLength)
            .IsRequired();
        
        builder.Property(x => x.Note)
            .HasConversion(x => x.Value,
                x => new(x))
            .IsRequired();
        
        builder.Property(x => x.Date)
            .HasConversion(x => x.Value,
                x => new(x))
            .IsRequired();
    }
    
}