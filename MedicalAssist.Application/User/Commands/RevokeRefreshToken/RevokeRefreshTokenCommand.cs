using MediatR;

namespace MedicalAssist.Application.User.Commands.RevokeRefreshToken;
public sealed record RevokeRefreshTokenCommand() : IRequest;
