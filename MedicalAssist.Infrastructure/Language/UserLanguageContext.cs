using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MedicalAssist.Infrastructure.Language;
internal sealed class UserLanguageContext : IUserLanguageContext
{
	private readonly IHttpContextAccessor _contextAccessor;

	public UserLanguageContext(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public Languages GetLanguage()
	{
		var langHeader = _contextAccessor.HttpContext?.Request.Headers["X-Current-Language"].FirstOrDefault();

		return langHeader switch
		{
			"pl-PL" => Languages.Polish,
			"en-US" => Languages.English,
			_ => throw new InvalidLanguageHeaderException(),
		};
	}
}
