using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions.RefreshToken;
public sealed class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException() : base("Given refresh token is invalid.")
    {

    }
}
