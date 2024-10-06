using MedicalAssistant.UI.Models.Settings;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface ISettingsService
{
	Task<Response<SettingsViewModel>> Get();
	Task<Response.Base.Response> Update(SettingsViewModel model);
}
