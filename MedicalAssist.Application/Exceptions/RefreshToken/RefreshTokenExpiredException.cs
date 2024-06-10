using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions.RefreshToken;
public sealed  class RefreshTokenExpiredException : BadRequestException
{
    public RefreshTokenExpiredException() : base("Refresh token has already expired.")
    {
        
    }
}
