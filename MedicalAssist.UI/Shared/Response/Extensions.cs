using System.Text.Json;
using MedicalAssist.UI.Shared.Response.Base;

namespace MedicalAssist.UI.Shared.Response;

public static class Extensions
{
	public async static Task<Base.Response> DeserializeResponse(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return new(true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;

		return new(false, errorDetails);
	}

	public async static Task<Response<T>> DeserializeResponse<T>(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			var content = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync())!;
			return new(content,true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;

		return new(default,false, errorDetails);
	}
}
