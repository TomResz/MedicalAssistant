namespace MedicalAssist.Domain.Exceptions.Shared;
public abstract class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
