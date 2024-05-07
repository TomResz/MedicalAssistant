namespace MedicalAssist.Application.Security;
public interface IEmailCodeManager
{
    bool Decode(string code,out string email);
    string Encode(string email);
}
