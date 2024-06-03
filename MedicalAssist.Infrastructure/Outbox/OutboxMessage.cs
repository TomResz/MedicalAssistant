namespace MedicalAssist.Infrastructure.Outbox;
public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string ContentJson { get; set; }

    public DateTime OccurredOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? ErrorMessageJson { get; set; }
}
