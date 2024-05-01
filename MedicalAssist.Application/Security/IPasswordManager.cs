namespace MedicalAssist.Application.Security;
public interface IPasswordManager
{
    bool IsValid(string password, string securedPassword);
    string Secure(string password);
}
