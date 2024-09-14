using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions.RefreshToken;
public sealed class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException() : base("Given refresh token is invalid.")
    {

    }
}
