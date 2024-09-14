using MedicalAssistant.Domain.Enums;

namespace MedicalAssistant.Application.Contracts;
public interface IUserLanguageContext
{
    public Languages GetLanguage();
}
