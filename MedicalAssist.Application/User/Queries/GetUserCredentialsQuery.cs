using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.User.Queries;
public sealed record GetUserCredentialsQuery() : IRequest<UserCredentialsDto>;
