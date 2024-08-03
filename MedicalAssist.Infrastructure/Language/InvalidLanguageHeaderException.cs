using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Infrastructure.Language;
public sealed class InvalidLanguageHeaderException : BadRequestException
{
    public InvalidLanguageHeaderException()
        : base("Unknown language header.")
    {
        
    }
}
