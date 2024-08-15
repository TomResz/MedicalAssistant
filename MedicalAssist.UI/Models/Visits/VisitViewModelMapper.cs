namespace MedicalAssist.UI.Models.Visits;

public static class VisitViewModelMapper
{
	public static VisitViewModel ToViewModel(this VisitDto visitDto) 
		=> new VisitViewModel
	{
		City = visitDto.Address.City,
		PostalCode = visitDto.Address.PostalCode,
		Street = visitDto.Address.Street,
		Date = visitDto.Date.Date,
		Time = visitDto.Date.TimeOfDay,
		PredictedEndDate = visitDto.End - visitDto.Date,
		DoctorName = visitDto.DoctorName,
		VisitDescription = visitDto.VisitDescription,
		VisitType = visitDto.VisitType,
	};
}
