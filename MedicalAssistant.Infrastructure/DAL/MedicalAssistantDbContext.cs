using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL;

public sealed class MedicalAssistantDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<MedicationRecommendation> Recommendations { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<UserVerification> UserVerifications { get; set; }
    public DbSet<VisitNotification> VisitNotifications { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<NotificationHistory> NotificationHistories { get; set; }
    public DbSet<TokenHolder> TokenHolders { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<MedicalHistory> MedicalHistories { get; set; }
    public DbSet<MedicationRecommendationNotification> MedicationRecommendationsNotifications { get; set; }
    public DbSet<MedicalNote> MedicalNotes { get; set; }
    public DbSet<DiseaseStage> DiseaseStages { get; set; }

    public DbSet<RecommendationUsage> RecommendationUsages { get; set; }

    public MedicalAssistantDbContext(DbContextOptions<MedicalAssistantDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}