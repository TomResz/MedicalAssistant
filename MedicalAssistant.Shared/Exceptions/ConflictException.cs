namespace MedicalAssistant.Domain.Exceptions;
public abstract class ConflictException : Exception
{
    public ConflictException(string message) : base(message)    
    {    
    }
}
