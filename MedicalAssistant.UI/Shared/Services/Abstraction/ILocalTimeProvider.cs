namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface ILocalTimeProvider
{
	Task<DateTime> FromUtcToLocal(DateTime dateTimeUtc);
	Task<DateTime> FromLocalToUtc(DateTime dateTimeLocal);
	Task<DateTime> CurrentDate();
	Task<double> TimeZoneOffset();
}
