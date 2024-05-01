using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace MedicalAssist.Infrastructure.Security;
internal sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordManager(IPasswordHasher<User> passwordHasher) => _passwordHasher = passwordHasher;

    public bool IsValid(string password, string securedPassword)
       => _passwordHasher.VerifyHashedPassword(default, securedPassword, password) == PasswordVerificationResult.Success;

    public string Secure(string password)
        => _passwordHasher.HashPassword(default, password);
}
