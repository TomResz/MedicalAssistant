using MedicalAssist.Domain.Exceptions;

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

public sealed class UserWithExternalProviderCannotChangePasswordException : BadRequestException
{
    public UserWithExternalProviderCannotChangePasswordException()
        : base("User with external authentication provider cannot change password.")
    {
        
    }
}