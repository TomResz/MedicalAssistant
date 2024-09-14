using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.Security;
public interface ICodeVerification
{
    string Generate(Date currentDate);
}
