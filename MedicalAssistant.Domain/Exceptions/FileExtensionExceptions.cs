namespace MedicalAssistant.Domain.Exceptions;

public class InvalidFileExtensionException : BadRequestException
{


    public InvalidFileExtensionException() : base($"Invalid file extension.Available extensions: .pdf, .png, .jpg.")
    {

    }
}