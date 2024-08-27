namespace MedicalAssist.UI.Models.Visits;

public static class VisitMapper
{
	public static CreateVisitVisitModel ToModel(this VisitViewModel viewModel)
	{
		var dateValue = viewModel.Date!.Value.Date;
		var defaultTime = TimeSpan.FromHours(12);
		var defaultVisitTime = TimeSpan.FromMinutes(30);
		var date = dateValue.Add(viewModel.Time ?? defaultTime);
		return new CreateVisitVisitModel()
		{
			City = viewModel.City,
			PostalCode = viewModel.PostalCode,
			Street = viewModel.Street,
			Date = date.ToString("yyyy-MM-dd HH:mm"),
			DoctorName = viewModel.DoctorName,
			VisitType = viewModel.VisitType,
			VisitDescription = viewModel.VisitDescription,
			PredictedEndDate = date.Add(viewModel.PredictedEndDate ?? defaultVisitTime).ToString("yyyy-MM-dd HH:mm")
		};
	}


}

