﻿using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalAssistant.Infrastructure.DAL.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany<Visit>()
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MedicalNotes)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.IsActive)
            .IsRequired(true);

        builder.HasQueryFilter(x => x.IsActive);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.DateOfDeactivationUtc)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired(false);

        builder.HasMany(x => x.MedicalHistories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);


        builder.HasMany(x => x.MedicationRecommendations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.NotificationHistories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RefreshTokens)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new(x))
            .IsRequired();

        builder.Navigation(x => x.UserVerification)
            .IsRequired(false);


        builder.Navigation(x => x.ExternalUserProvider)
            .IsRequired(false);

        builder.HasOne(x => x.UserSettings)
            .WithOne(x => x.User)
            .HasForeignKey<UserSettings>(x => x.UserId);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Domain.ValueObjects.Email(x))
            .IsRequired();

        builder.Property(x => x.Password)
            .HasConversion(x => x != null ? x.Value : null, x => new Password(x))
            .IsRequired(false);

        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new FullName(x))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion(x => x.Value, x => new Role(x))
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasConversion(x => x.Value, x => new Date(x))
            .IsRequired();
    }
}