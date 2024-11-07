using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MedicalAssistant.Infrastructure.Security;
internal sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordManager(IPasswordHasher<User> passwordHasher) => _passwordHasher = passwordHasher;

    public bool IsValid(string password, string securedPassword)
       => _passwordHasher.VerifyHashedPassword(default, securedPassword, password) == PasswordVerificationResult.Success;

    public string Secure(string password)
        => _passwordHasher.HashPassword(default, password);
}
