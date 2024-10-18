using MedicalAssistant.UI.Models.Medication;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public static class MedicationMappers
{
	private const string Morning = "morning";
	private const string Afternoon = "afternoon";
	private const string Evening = "evening";
	private const string Nigth = "night";

	public static AddMedicationModel ToInsertRequest(this MedicationViewModel viewModel)
	{
		var timesOfDay = new List<string>(capacity: 4);

		if (viewModel.MorningChecked)
			timesOfDay.Add(Morning);

		if (viewModel.AfternoonChecked)
			timesOfDay.Add(Afternoon);

		if (viewModel.EveningChecked)
			timesOfDay.Add(Evening);

		if (viewModel.NightChecked)
			timesOfDay.Add(Nigth);

		return new()
		{
			VisitId = viewModel.VisitId,
			EndDate = (DateTime)viewModel.DateRange.End!,
			StartDate = (DateTime)viewModel.DateRange.Start!,
			ExtraNote = viewModel.ExtraNote,
			MedicineName = viewModel.MedicineName,
			Quantity = viewModel.Quantity,
			TimeOfDay = [.. timesOfDay]
		};
	}

	public static MedicationViewModel ToViewModel(this AddMedicationModel model)
	{
		return new MedicationViewModel
		{
			VisitId = model.VisitId,
			DateRange = new DateRange
			{
				Start = model.StartDate,
				End = model.EndDate
			},
			ExtraNote = model.ExtraNote,
			MedicineName = model.MedicineName,
			Quantity = model.Quantity,
			MorningChecked = model.TimeOfDay.Contains(Morning),
			AfternoonChecked = model.TimeOfDay.Contains(Afternoon),
			EveningChecked = model.TimeOfDay.Contains(Evening),
			NightChecked = model.TimeOfDay.Contains(Nigth)
		};
	}
}
