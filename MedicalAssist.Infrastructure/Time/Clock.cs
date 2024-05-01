using MedicalAssist.Domain.Abstraction;

namespace MedicalAssist.Infrastructure.Time;
internal sealed class Clock : IClock
{
    public DateTime GetCurrentUtc() => DateTime.UtcNow;
}
