using MediatR;

namespace MedicalAssistant.Application.User.Commands.RevokeRefreshToken;
public sealed record RevokeRefreshTokenCommand(
	string RefreshToken) : IRequest;
