using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Contracts;
public interface IGoogleAuthService
{
	Task<ExternalApiResponse?> AuthenticateUser(string code, CancellationToken ct);
}
