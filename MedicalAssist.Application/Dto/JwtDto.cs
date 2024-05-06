namespace MedicalAssist.Application.Dto;
public class JwtDto
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}
