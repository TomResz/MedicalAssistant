using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions.RefreshToken;
public sealed  class RefreshTokenExpiredException : BadRequestException
{
    public RefreshTokenExpiredException() : base("Refresh token has already expired.")
    {
        
    }
}
