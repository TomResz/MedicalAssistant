using MedicalAssistant.Domain.Enums;

namespace MedicalAssistant.Infrastructure.PDF.Translation;

internal class Translations(Languages language)
{
        private readonly Dictionary<Languages, Dictionary<string, string>> _translations = new()
        {
            {
                Languages.Polish, new Dictionary<string, string>
                {
                    { TranslationKeys.VisitReportTitle, "Raport Z Wizyt" },
                    { TranslationKeys.VisitDetails, "Szczegóły Wizyty" },
                    { TranslationKeys.Doctor, "Lekarz" },
                    { TranslationKeys.Type, "Typ" },
                    { TranslationKeys.Date, "Data" },
                    { TranslationKeys.Description, "Opis" },
                    { TranslationKeys.MedicalRecommendations, "Rekomendacje Medyczne" },
                    { TranslationKeys.Medicine, "Lek" },
                    { TranslationKeys.Quantity, "Ilość" },
                    { TranslationKeys.TimeOfDay, "Pory dnia" },
                    { TranslationKeys.Note, "Notatka" },
                    { TranslationKeys.StartDate, "Data rozpoczęcia" },
                    { TranslationKeys.EndDate, "Data zakończenia" },
                    { TranslationKeys.Footer, "© 2024 Asystent Medyczny - Wszystkie prawa zastrzeżone" }
                }
            },
            {
                Languages.English, new Dictionary<string, string>
                {
                    { TranslationKeys.VisitReportTitle, "Visit Report" },
                    { TranslationKeys.VisitDetails, "Visit Details" },
                    { TranslationKeys.Doctor, "Doctor" },
                    { TranslationKeys.Type, "Type" },
                    { TranslationKeys.Date, "Date" },
                    { TranslationKeys.Description, "Description" },
                    { TranslationKeys.MedicalRecommendations, "Medical Recommendations" },
                    { TranslationKeys.Medicine, "Medicine" },
                    { TranslationKeys.Quantity, "Quantity" },
                    { TranslationKeys.TimeOfDay, "Times of Day" },
                    { TranslationKeys.Note, "Note" },
                    { TranslationKeys.StartDate, "Start Date" },
                    { TranslationKeys.EndDate, "End Date" },
                    { TranslationKeys.Footer, "© 2024 Medical Assistant - All rights reserved" }
                }
            }
        };

    protected string Translate(string key)
    {
        if (_translations.TryGetValue(language, out var langTranslations) &&
            langTranslations.TryGetValue(key, out var text))
        {
            return text;
        }

        return key;
    }
}