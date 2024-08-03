using MedicalAssist.Domain.Enums;
using MedicalAssist.Infrastructure.Email.Bodies;
using MedicalAssist.Infrastructure.Language;

namespace MedicalAssist.Infrastructure.Email.Factory;
internal static class EmailBodyFactory
{
	internal static IEmailBody Create(Languages language)
		=> language switch
		{
			Languages.Polish => new PolishEmailHtmlBodies(),
			Languages.English => new EnglishEmailHtmlBodies(),
			_ => throw new InvalidLanguageHeaderException()
		};

}
