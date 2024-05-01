using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Security;
public interface IAuthenticator
{
    JwtDto GenerateToken(Domain.Entites.User user);
}
