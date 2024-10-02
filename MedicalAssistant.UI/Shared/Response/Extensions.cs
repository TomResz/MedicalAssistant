using System.Text.Json;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Response;

public static class Extensions
{
	public async static Task<Base.Response> DeserializeResponse(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return new(true);
		}
		var json = await response.Content.ReadAsStringAsync();
		
		if (string.IsNullOrEmpty(json))
		{
			return new(false);
		}

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(json,options)!;

		return new(false, errorDetails);
	}

	public async static Task<Response<T>> DeserializeResponse<T>(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			var json = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrEmpty(json))
			{
				return new(default, true);
			}

			var content = JsonSerializer.Deserialize<T?>(json,
								new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
			return new(content,true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;

		return new(default,false, errorDetails);
	}
}
