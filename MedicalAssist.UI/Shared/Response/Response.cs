namespace MedicalAssist.UI.Shared.Response;

public class Response
{
	public bool IsSuccess { get; private set; }
	public bool IsFailure => !IsSuccess;

	public Response(bool isSuccess,BaseErrorDetails? errorDetails = null)
	{
		IsSuccess = isSuccess;
		ErrorDetails = errorDetails;
	}
	public BaseErrorDetails? ErrorDetails { get; }
}

public sealed class Response<T> : Response
{
	public T? Value { get; private set; }

	public Response(T? value, bool isSuccess,BaseErrorDetails? errorDetails =null) : base(isSuccess, errorDetails)
	{
		Value = value;
	}
}
