namespace MedicalAssist.UI.Shared.Requests;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
    public string OldAccessToken { get; set; }
}
