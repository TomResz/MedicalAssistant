using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Application.Security;
public interface ICodeVerification
{
    string Generate(Date currentDate);
}
