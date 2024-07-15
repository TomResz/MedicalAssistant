using MedicalAssist.Application.Dto;

namespace MedicalAssist.Infrastructure.ExternalProviders.Google;
internal static class GoogleDtoMapper
{
    public static ExternalApiResponse ToDto(this GoogleDataResponse response)
        => new(response.Id, response.Email, response.FullName);
}
