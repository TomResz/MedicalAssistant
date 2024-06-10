using MediatR;

namespace MedicalAssist.Application.User.Commands.RefreshToken;
public sealed record RefreshTokenCommand(
    string RefreshToken,
    string OldAccessToken) : IRequest<RefreshTokenResponse>;

