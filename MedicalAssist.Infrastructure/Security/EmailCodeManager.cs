using MedicalAssist.Application.Security;
using System.Text;

namespace MedicalAssist.Infrastructure.Security;
internal sealed class EmailCodeManager : IEmailCodeManager
{
    public bool Decode(string code, out string email)
    {
        email = "";
        var bytes = Convert.FromBase64String(code);
        var inputText= Encoding.UTF8.GetString(bytes);
        try
        {
            var parsedEmail = new Domain.ValueObjects.Email(inputText);
            email = parsedEmail;
        }
        catch(Exception) 
        { 
            return false;
        }

        return true;
    }

    public string Encode(string email)
    {
        var text = Encoding.UTF8.GetBytes(email);
        return Convert.ToBase64String(text);
    }
}
