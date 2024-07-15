using MedicalAssist.Application.Dto;

namespace MedicalAssist.Infrastructure.ExternalProviders.Facebook;
internal static class FacebookDataMapper
{
	public static ExternalApiResponse ToDto(this FacebookDataResponse response)
		=> new(response.Id, response.Email, response.FullName);

}
