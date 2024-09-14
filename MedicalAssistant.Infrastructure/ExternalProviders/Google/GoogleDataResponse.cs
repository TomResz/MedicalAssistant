using System.Text.Json.Serialization;

namespace MedicalAssistant.Infrastructure.ExternalProviders.Google;
internal sealed class GoogleDataResponse
{
    [JsonPropertyName("sub")]
    public string Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("name")]
    public string FullName { get; set; }
}
