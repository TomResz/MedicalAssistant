using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyRefreshTokenException : BadRequestException
{
    public EmptyRefreshTokenException() : base("Refresh token cannot be empty.")
    {
        
    }
}
