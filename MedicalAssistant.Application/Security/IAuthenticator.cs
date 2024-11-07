using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Security;
public interface IAuthenticator
{
    JwtDto GenerateToken(Domain.Entities.User user);
}
