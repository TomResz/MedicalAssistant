using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.RefreshToken;
public sealed record RefreshTokenCommand(
    string RefreshToken,
    string OldAccessToken) : ICommand<RefreshTokenResponse>;

