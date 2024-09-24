using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.JSInterop;

namespace MedicalAssistant.UI.Shared.Services.Time;

public sealed class LocalTimeProvider : ILocalTimeProvider
{
	private readonly IJSRuntime _jsRuntime;

	public LocalTimeProvider(IJSRuntime jSRuntime)
	{
		_jsRuntime = jSRuntime;
	}

	public async Task<DateTime> CurrentDate()
	{
		var date = await _jsRuntime.InvokeAsync<string>("getCurrentBrowserDate");
		return DateTime.Parse(date);
	}

	public async Task<DateTime> FromLocalToUtc(DateTime dateTimeLocal)
	{
		var timezoneOffsetInMinutes = await _jsRuntime.InvokeAsync<int>("getTimezoneOffsetInMinutes");
		var offset = TimeSpan.FromMinutes(timezoneOffsetInMinutes);
		return dateTimeLocal.Add(offset);
	}

	public async Task<DateTime> FromUtcToLocal(DateTime dateTimeUtc)
	{
		var timezoneOffsetInMinutes = await _jsRuntime.InvokeAsync<int>("getTimezoneOffsetInMinutes");
		var offset = TimeSpan.FromMinutes(-timezoneOffsetInMinutes);
		return dateTimeUtc.Add(offset);
	}
}
