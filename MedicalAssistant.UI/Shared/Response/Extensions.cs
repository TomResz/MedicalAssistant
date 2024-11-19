using System.Text.Json;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Response;

public static class Extensions
{
	private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new TimeOnlyJsonConverter() }
	};

	public static async Task<Base.Response> DeserializeResponse(this HttpResponseMessage response)
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


		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(json, Options)!;

		return new(false, errorDetails);
	}

	public static async Task<Response<T>> DeserializeResponse<T>(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			var json = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrEmpty(json))
			{
				return new Response<T>(default, true);
			}

			var content = JsonSerializer.Deserialize<T?>(json,Options)!;
			return new Response<T>(content, true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;

		return new(default, false, errorDetails);
	}
}
