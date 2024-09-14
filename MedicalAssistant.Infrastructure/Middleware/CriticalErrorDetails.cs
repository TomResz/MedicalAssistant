namespace MedicalAssistant.Infrastructure.Middleware;
public sealed class CriticalErrorDetails 
{ 
    public ErrorDetails ErrorDetails { get; set; }
    public string StackTrace { get; set; }
    public CriticalErrorDetails(ErrorDetails errorDetails,string stackTrace) 
	{
		ErrorDetails = errorDetails;
		StackTrace = stackTrace;
	}
}
