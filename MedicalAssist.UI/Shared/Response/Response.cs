namespace MedicalAssist.UI.Shared.Response;

public class Response
{
	public bool IsSuccess { get; private set; }
	public bool IsFailure => !IsSuccess;
	public Error Error { get; }

	public Response(bool isSuccess, Error error)
	{
		if ((isSuccess && error != Error.None) ||
			(!isSuccess && error == Error.None))
		{
			throw new Exception();
		}
		IsSuccess = isSuccess;
		Error = error;
	}

}

public sealed class Response<T> : Response
{
	public T? Value { get; private set; }
	public Response(T? value, bool isSuccess,Error error) : base(isSuccess, error)
	{
		Value = value;
	}
}
