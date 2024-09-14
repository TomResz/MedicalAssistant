using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Infrastructure.Language;
public sealed class InvalidLanguageHeaderException : BadRequestException
{
    public InvalidLanguageHeaderException()
        : base("Unknown language header.")
    {
        
    }
}
