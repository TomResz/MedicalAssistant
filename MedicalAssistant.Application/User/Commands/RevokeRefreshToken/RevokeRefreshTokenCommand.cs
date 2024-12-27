using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.RevokeRefreshToken;
public sealed record RevokeRefreshTokenCommand(
	string RefreshToken) : ICommand;
