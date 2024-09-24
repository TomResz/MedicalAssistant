using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;
using System.Text.RegularExpressions;

namespace MedicalAssistant.UI.Models.Visits;

public class VisitViewModelValidator : BaseValidator<VisitViewModel>
{
	private readonly Regex _postalCodeRegex = new("\\d{2}-\\d{3}", RegexOptions.Compiled);
	public VisitViewModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.City)
			.NotEmpty()
			.WithMessage(Translations.CityCannotBeEmpty);

		RuleFor(x => x.PostalCode)
			.NotEmpty()
			.WithMessage(Translations.IncorrectPostalCode)
			.Must(_postalCodeRegex.IsMatch)
			.WithMessage(Translations.IncorrectPostalCode);

		RuleFor(x => x.VisitType)
			.NotEmpty()
			.WithMessage(Translations.EmptyVisitType);

		RuleFor(x => x.Street)
			.NotEmpty()
			.WithMessage(Translations.EmptyStreet);

		RuleFor(x => x.Date)
			.NotEmpty()
			.WithMessage(Translations.EmptyDate);

		RuleFor(x => x.Time)
			.NotEmpty()
			.WithMessage(Translations.EmptyVisitType);

		RuleFor(x => x.DoctorName)
			.NotEmpty()
			.WithMessage(Translations.EmptyDoctorName);

		RuleFor(x => x.VisitDescription)
			.NotEmpty()
			.WithMessage(Translations.EmptyVisitDescription);

		RuleFor(x => x.PredictedEndDate)
			.NotEmpty()
			.WithMessage(Translations.EmptyPredictedTime)
			.Must(IsPredictedTimeValid)
			.WithMessage(Translations.EmptyPredictedTime);
	}
	private bool IsPredictedTimeValid(TimeSpan? time) 
		=> time is not null && time.Value > TimeSpan.FromSeconds(0);
}
