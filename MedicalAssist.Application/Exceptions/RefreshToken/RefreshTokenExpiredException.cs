using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions.RefreshToken;
public sealed  class RefreshTokenExpiredException : BadRequestException
{
    public RefreshTokenExpiredException() : base("Refresh token has already expired.")
    {
        
    }
}
