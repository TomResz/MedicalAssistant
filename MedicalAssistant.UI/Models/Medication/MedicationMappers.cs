using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Visits;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public static class MedicationMappers
{
	public const string Morning = "morning";
	public const string Afternoon = "afternoon";
	public const string Evening = "evening";
	public const string Night = "night";

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
			timesOfDay.Add(Night);

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

	public static UpdateMedicationModel ToUpdateRequest(this MedicationViewModel viewModel,Guid id)
	{
		List<string> timesOfDay = GetTimesOfDayArray(viewModel);

		return new()
		{
			Id = id,
			VisitId = viewModel.VisitId,
			EndDate = (DateTime)viewModel.DateRange.End!,
			StartDate = (DateTime)viewModel.DateRange.Start!,
			ExtraNote = viewModel.ExtraNote,
			MedicineName = viewModel.MedicineName,
			Quantity = viewModel.Quantity,
			TimeOfDay = [.. timesOfDay]
		};
	}

	private static List<string> GetTimesOfDayArray(MedicationViewModel viewModel)
	{
		var timesOfDay = new List<string>(capacity: 4);

		if (viewModel.MorningChecked)
			timesOfDay.Add(Morning);

		if (viewModel.AfternoonChecked)
			timesOfDay.Add(Afternoon);

		if (viewModel.EveningChecked)
			timesOfDay.Add(Evening);

		if (viewModel.NightChecked)
			timesOfDay.Add(Night);
		return timesOfDay;
	}

	public static MedicationDto ToDto(this MedicationViewModel viewModel,VisitDto? visitDto,Guid id)
	{
		return new MedicationDto
		{
			Visit = visitDto,
			EndDate = (DateTime)viewModel.DateRange.End!,
			StartDate = (DateTime)viewModel.DateRange.Start!,
			ExtraNote = viewModel.ExtraNote,
			Id = id,
			Name = viewModel.MedicineName,
			Quantity = viewModel.Quantity,
			TimeOfDay = [.. GetTimesOfDayArray(viewModel)]
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
			NightChecked = model.TimeOfDay.Contains(Night)
		};
	}

	public static MedicationViewModel ToViewModel(this MedicationDto medicationDto)
	{
		return new MedicationViewModel
		{
			VisitId = medicationDto.Visit?.Id,
			AfternoonChecked = medicationDto.TimeOfDay.Contains(Afternoon),
			MorningChecked= medicationDto.TimeOfDay.Contains(Morning),
			EveningChecked = medicationDto.TimeOfDay.Contains(Evening),
			NightChecked = medicationDto.TimeOfDay.Contains(Night),
			DateRange = new(medicationDto.StartDate,medicationDto.EndDate),
			ExtraNote = medicationDto.ExtraNote,
			MedicineName = medicationDto.Name,	
			Quantity = medicationDto.Quantity,
		};
	}
}
