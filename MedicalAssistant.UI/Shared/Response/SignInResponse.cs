using System.Text.Json.Serialization;

namespace MedicalAssistant.UI.Shared.Response;

public class SignInResponse
{
	[JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

	[JsonPropertyName("refreshToken")]
	public string RefreshToken { get; set; }
}
