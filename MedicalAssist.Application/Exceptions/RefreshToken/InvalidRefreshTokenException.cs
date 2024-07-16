using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions.RefreshToken;
public sealed class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException() : base("Given refresh token is invalid.")
    {

    }
}
