using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;

public sealed class InactiveVerificationCodeException : BadRequestException
{
    public InactiveVerificationCodeException() : base("The verification code has expired.")
    {
        
    }
}
public sealed class AccountIsAlreadyVerifiedException : BadRequestException
{
    public AccountIsAlreadyVerifiedException() : base("This account is already verified.")
    {
        
    }
}