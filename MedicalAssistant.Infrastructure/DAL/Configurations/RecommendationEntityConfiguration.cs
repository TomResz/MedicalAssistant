﻿using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class RecommendationEntityConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.HasKey(x=> x.Id);

        builder.Property(x => x.VisitId)
            .HasConversion(
                x => x.Value,
                x => new(x))
            .IsRequired();

        builder.HasOne<Visit>()
            .WithMany(x=>x.Recommendations)
            .HasForeignKey(x => x.VisitId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Property(x=> x.Id)
            .HasConversion(x=> x.Value, x=> new RecommendationId(x))
            .IsRequired();

        builder.Property(x => x.ExtraNote)
            .HasConversion(x => x.Value, x => new Note(x))
            .HasMaxLength(500)
            .IsRequired(false);
           
        builder.Property(x=> x.CreatedAt)
            .HasConversion(x=> x.Value, x => new Date(x))
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasConversion(x => x.Value, x => new Date(x))
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasConversion(x => x.Value, x => new Date(x))
            .IsRequired();

        builder.ComplexProperty(x => x.Medicine, conf =>
        {
            conf.Property(x => x.TimeOfDay)
                .HasConversion(x => x.Value, x => new TimeOfDay(x))
                .IsRequired();

            conf.Property(x => x.Name)
                .HasConversion(x => x.Value, x => new MedicineName(x))
                .IsRequired();

            conf.Property(x => x.Quantity)
                .HasConversion(x => x.Value, x => new Quantity(x))
                .IsRequired();
        });
    }
}