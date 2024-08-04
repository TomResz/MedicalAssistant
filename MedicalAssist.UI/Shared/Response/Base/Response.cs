namespace MedicalAssist.UI.Shared.Response.Base;

public class Response
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public BaseErrorDetails? ErrorDetails { get; init; }


    public Response(bool isSuccess, BaseErrorDetails? errorDetails = null)
    {
        if (isSuccess &&
            errorDetails is not null)
        {
            throw new InvalidOperationException();
        }
        IsSuccess = isSuccess;
        ErrorDetails = errorDetails;
    }
}

public sealed class Response<T> : Response
{
    public T? Value { get; private set; }

    public Response(T? value, bool isSuccess, BaseErrorDetails? errorDetails = null) : base(isSuccess, errorDetails)
        => Value = value;
}