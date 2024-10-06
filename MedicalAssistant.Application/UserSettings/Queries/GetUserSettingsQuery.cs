using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.UserSettings.Queries;
public sealed record GetUserSettingsQuery() 
	: IRequest<SettingsDto>;
