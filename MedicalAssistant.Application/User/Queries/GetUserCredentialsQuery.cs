using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.User.Queries;
public sealed record GetUserCredentialsQuery() : IQuery<UserCredentialsDto>;
