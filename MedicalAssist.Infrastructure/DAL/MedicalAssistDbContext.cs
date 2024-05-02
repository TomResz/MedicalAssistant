using MedicalAssist.Domain.Entites;
using MedicalAssist.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL;
internal sealed class MedicalAssistDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<UserVerification> UserVerifications { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public MedicalAssistDbContext(DbContextOptions<MedicalAssistDbContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);   
    }
}
