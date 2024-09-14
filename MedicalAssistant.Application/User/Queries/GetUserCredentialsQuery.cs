using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.User.Queries;
public sealed record GetUserCredentialsQuery() : IRequest<UserCredentialsDto>;
