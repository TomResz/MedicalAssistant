namespace MedicalAssistant.UI.Shared.Extensions;

public static class StartOfWeekDatetime
{
    public static DateTime StartOfWeek(this DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }
}