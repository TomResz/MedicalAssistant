using System.Text.Json;

namespace MedicalAssist.Infrastructure.Middleware;
public sealed class CriticalErrorDetails : ErrorDetails
{
    public string StackTrace { get; set; }
    public CriticalErrorDetails(int code, string type, string message, string stackTrace) : base(code, type, message)
	{
		StackTrace = stackTrace;
	}
	public override string ToString()
	{
		return JsonSerializer.Serialize(this);
	}
}
