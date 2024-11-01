using MedicalAssistant.UI.Components.Visits;
using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class MedicationPage
{
	private bool _isMedicationLoading = true;
	private bool _notificationsLoading = true;

	private MedicationDto _medication = new();
	private List<MedicationNotificationWithDateRangeDto> _notifications = [];


	[Parameter]
	public Guid Id { get; set; }

	[Inject]
	public IMedicationNotificationService MedicationNotificationService { get; set; }

	[Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Inject]
	public NavigationManager NavigationManager { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var offset = await LocalTimeProvider.TimeZoneOffset();

		var response = await MedicationNotificationService.Get(Id, offset);

		if (response.IsSuccess)
		{
			_notifications = response.Value!;
			_notificationsLoading = false;
		}

		var responseMedication = await MedicationService.GetById(Id);

		if (responseMedication.IsSuccess)
		{
			_medication = responseMedication.Value!;
			_isMedicationLoading = false;
		}
		else
		{
			NavigationManager.NavigateTo("/");
			Snackbar.Add(Translations.SomethingWentWrong,Severity.Error);
			return;
		}
	}
}
