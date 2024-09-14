using MediatR;

namespace MedicalAssistant.Application.User.Commands.RefreshToken;
public sealed record RefreshTokenCommand(
    string RefreshToken,
    string OldAccessToken) : IRequest<RefreshTokenResponse>;

