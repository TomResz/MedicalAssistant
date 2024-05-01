using MedicalAssist.Application.Dto;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
internal static class UserToUserCredentialsDto
{
	public static UserCredentialsDto ToUserCredentialsDto(this Domain.Entites.User user) 
		=> new()
	{
		UserId = user.Id,
		Email = user.Email,
		FullName = user.FullName,
	};
}
