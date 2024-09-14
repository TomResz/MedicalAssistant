using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.Email.Bodies;
using MedicalAssistant.Infrastructure.Language;

namespace MedicalAssistant.Infrastructure.Email.Factory;
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
