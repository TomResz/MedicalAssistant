using System.Text.Json.Serialization;

namespace MedicalAssistant.Infrastructure.ExternalProviders.Facebook;
internal class FacebookDataResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("email")]
	public string Email { get; set; }

	[JsonPropertyName("name")]
	public string FullName { get; set; }
}
