using System.Text.Json;

namespace MedicalAssist.Infrastructure.Middleware;
public sealed class CriticalErrorDetails 
{ 
    public ErrorDetails ErrorDetails { get; set; }
    public string StackTrace { get; set; }
    public CriticalErrorDetails(ErrorDetails errorDetails,string stackTrace) 
	{
		ErrorDetails = errorDetails;
		StackTrace = stackTrace;
	}
	public override string ToString()
	{
		return JsonSerializer.Serialize(this);
	}
}
