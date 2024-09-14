namespace MedicalAssistant.Domain.Abstraction;
public interface IClock
{
    DateTime GetCurrentUtc();
}
