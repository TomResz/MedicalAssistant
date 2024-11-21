using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;
internal sealed class VisitEntityConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne<User>()
            .WithMany(x => x.Visits)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                    x => new(x))
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasConversion(
            x => x.Value,
            x => new UserId(x))
            .IsRequired();

        builder.Property(x => x.Date)
            .HasConversion(x => x.Value, x => new Date(x));

        builder.Property(x => x.PredictedEndDate)
            .HasConversion(x => x.Value, x => new Date(x));

        builder.ComplexProperty(x => x.Address, conf =>
        {
            conf.Property(x => x.PostalCode)
                .HasConversion(postalCode => postalCode.Value, value => new PostalCode(value))
                .HasMaxLength(6)
                .IsRequired();

            conf.Property(x => x.Street)
                .HasConversion(street => street.Value, value => new Street(value))
                .IsRequired();

            conf.Property(x => x.City)
                .HasConversion(city => city.Value, value => new City(value))
                .IsRequired();
        });

        builder.Property(x => x.DoctorName)
            .HasConversion(x => x.Value, x => new DoctorName(x))
            .HasMaxLength(DoctorName.MaxLength)
            .IsRequired();

        builder.Property(x => x.VisitType)
            .HasConversion(x => x.Value, x => new VisitType(x))
            .HasMaxLength(VisitType.MaxLength)
            .IsRequired();


        builder.Property(x => x.VisitDescription)
            .HasConversion(x => x.Value, x => new VisitDescription(x))
            .HasMaxLength(VisitDescription.MaxLength)
            .IsRequired();


        builder.HasIndex(b => new
        {
            b.VisitType,
            b.VisitDescription,
            b.DoctorName
        })
        .HasMethod("GIN")
        .IsTsVectorExpressionIndex("english");

	}
}
