using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;
public interface IFacebookAuthService
{
	Task<ExternalApiResponse?> AuthenticateUser(string code, CancellationToken ct);
}
