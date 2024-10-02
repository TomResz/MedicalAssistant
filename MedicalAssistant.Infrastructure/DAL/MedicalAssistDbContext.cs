﻿using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL;
internal sealed class MedicalAssistDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<UserVerification> UserVerifications { get; set; }
    public DbSet<VisitNotification> VisitNotifications { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<NotificationHistory> NotificationHistories { get; set; }
    public DbSet<TokenHolder> TokenHolders { get; set; }
    public MedicalAssistDbContext(DbContextOptions<MedicalAssistDbContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);   
    }
}
