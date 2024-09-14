using MedicalAssistant.Domain.Exceptions;
using System.Drawing;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidNewPasswordException : BadRequestException
{
    public InvalidNewPasswordException() : base("The new password cannot be the same as the old one.")
    {
        
    }
}
