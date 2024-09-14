using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyRefreshTokenException : BadRequestException
{
    public EmptyRefreshTokenException() : base("Refresh token cannot be empty.")
    {
        
    }
}
