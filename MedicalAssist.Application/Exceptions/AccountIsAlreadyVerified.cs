using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class AccountIsAlreadyVerified : BadRequestException
{
    public AccountIsAlreadyVerified() : base("Account with given email is already verified.")
    {
        
    }
}
