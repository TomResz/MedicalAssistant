using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class AccountIsAlreadyVerified : BadRequestException
{
    public AccountIsAlreadyVerified() : base("Account with given email is already verified.")
    {
        
    }
}
