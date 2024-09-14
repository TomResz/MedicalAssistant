using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.Email.EmailSubjects;
using MedicalAssistant.Infrastructure.Language;

namespace MedicalAssistant.Infrastructure.Email.Factory;
internal static class EmailSubjectFactory
{
    internal static IEmailSubject Create(Languages languages)
        => languages switch
        {
            Languages.Polish => new PolishEmailSubjects(),
            Languages.English => new EnglishEmailSubjects(),
            _ => throw new InvalidLanguageHeaderException()
		};
}
