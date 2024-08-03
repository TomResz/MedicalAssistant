using MedicalAssist.Domain.Enums;
using MedicalAssist.Infrastructure.Email.EmailSubjects;
using MedicalAssist.Infrastructure.Language;

namespace MedicalAssist.Infrastructure.Email.Factory;
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
