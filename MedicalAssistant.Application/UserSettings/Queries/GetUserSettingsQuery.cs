using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.UserSettings.Queries;
public sealed record GetUserSettingsQuery() 
	: IQuery<SettingsDto>;
