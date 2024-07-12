using System.Text.Json;

namespace MedicalAssist.Infrastructure.Middleware;

public class ErrorDetails
{
    public ErrorDetails(int code,string type,string message)
    {
        Code = code;
		Type = type;
		Message = message;
    }
    public int Code { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }

	public override string ToString()
	{
        return JsonSerializer.Serialize(this);
	}
}