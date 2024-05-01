namespace MedicalAssist.Domain.Abstraction;
public interface IClock
{
    DateTime GetCurrentUtc();
}
