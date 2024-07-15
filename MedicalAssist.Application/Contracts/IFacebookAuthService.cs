using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Contracts;
public interface IFacebookAuthService
{
	Task<ExternalApiResponse?> AuthenticateUser(string code, CancellationToken ct);
}
