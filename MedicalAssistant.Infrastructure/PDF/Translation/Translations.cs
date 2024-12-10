using System.Text;
using MedicalAssistant.Domain.Enums;

namespace MedicalAssistant.Infrastructure.PDF.Translation;

internal class Translations(Languages language)
{
    private readonly Dictionary<Languages, Dictionary<TranslationKeys, string>> _translations = new()
    {
        {
            Languages.Polish, new Dictionary<TranslationKeys, string>
            {
                { TranslationKeys.Notes,"Notatki" },
                { TranslationKeys.Tags,"Tagi" },
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
                { TranslationKeys.Footer, "© 2024 Asystent Medyczny - Wszystkie prawa zastrzeżone" },
                { TranslationKeys.Morning, "Rano" },
                { TranslationKeys.Afternoon, "Popołudniu" },
                { TranslationKeys.Evening, "Wieczór" },
                { TranslationKeys.Night, "Noc" },
                { TranslationKeys.MedicalHistoryTitle, "Historia Chorób" },
                {TranslationKeys.DiseaseName,"Nazwa Choroby"},
                {TranslationKeys.SymptomDescription, "Opis Objawów"},
                {TranslationKeys.DiseaseDetails,"Szczegóły Choroby"},
                {TranslationKeys.StageName,"Nazwa Etapu"},
                {TranslationKeys.DiseaseStagesTitle,"Etapy Choroby"},
            }
        },
        {
            Languages.English, new Dictionary<TranslationKeys, string>
            {
                { TranslationKeys.Notes,"Notes" },
                { TranslationKeys.Tags,"Tags" },
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
                { TranslationKeys.Footer, "© 2024 Medical Assistant - All rights reserved" },
                { TranslationKeys.Morning, "Morning" },
                { TranslationKeys.Afternoon, "Afternoon" },
                { TranslationKeys.Evening, "Evening" },
                { TranslationKeys.Night, "Night" },
                { TranslationKeys.MedicalHistoryTitle, "Disease History" },
                {TranslationKeys.DiseaseName,"Disease Name"},
                {TranslationKeys.SymptomDescription, "Symptom Description"},
                {TranslationKeys.DiseaseDetails,"Disease Details"},
                {TranslationKeys.StageName,"Stage Name"},
                {TranslationKeys.DiseaseStagesTitle,"Disease Stages"},

            }
        }
    };

    protected string TranslateTimesOfDay(string[] timesOfDay)
    {
        var sb = new StringBuilder();

        foreach (var time in timesOfDay)
        {
            switch (time)
            {
                case "morning":
                    sb.Append($"{Translate(TranslationKeys.Morning)}, ");
                    break;
                case "afternoon":
                    sb.Append($"{Translate(TranslationKeys.Afternoon)}, ");
                    break;
                case "evening":
                    sb.Append($"{Translate(TranslationKeys.Evening)}, ");
                    break;
                case "night":
                    sb.Append($"{Translate(TranslationKeys.Night)}, ");
                    break;
            }
        }

        var result = sb.ToString();

        return result.Substring(0, result.Length - 2);
    }


    protected string Translate(TranslationKeys key)
    {
        if (_translations.TryGetValue(language, out var langTranslations) &&
            langTranslations.TryGetValue(key, out var text))
        {
            return text;
        }

        return string.Empty;
    }
}