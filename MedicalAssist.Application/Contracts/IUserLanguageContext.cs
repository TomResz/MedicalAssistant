using MedicalAssist.Domain.Enums;

namespace MedicalAssist.Application.Contracts;
public interface IUserLanguageContext
{
    public Languages GetLanguage();
}
