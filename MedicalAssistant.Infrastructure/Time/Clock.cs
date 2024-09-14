using MedicalAssistant.Domain.Abstraction;

namespace MedicalAssistant.Infrastructure.Time;
internal sealed class Clock : IClock
{
    public DateTime GetCurrentUtc() => DateTime.UtcNow;
}
