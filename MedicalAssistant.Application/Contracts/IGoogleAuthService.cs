using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;
public interface IGoogleAuthService
{
	Task<ExternalApiResponse?> AuthenticateUser(string code, CancellationToken ct);
}
