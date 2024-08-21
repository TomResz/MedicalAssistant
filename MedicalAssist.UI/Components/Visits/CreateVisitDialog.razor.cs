using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssist.UI.Components.Visits;

public partial class CreateVisitDialog
{
	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }

	[CascadingParameter]
	private MudDialogInstance MudDialog { get; set; }

	[Parameter]
	public DateTime? SelectedDate { get; set; }

	[Parameter]
	public EventCallback<VisitDto> OnVisitCreated { get; set; }

	private readonly VisitViewModelValidator _validator = new();
	private readonly VisitViewModel _visit = new();

	private MudForm form;
	private bool _btnPressed = false;
	protected override void OnInitialized()
	{
		_visit.Date = SelectedDate;
	}

	private async Task SubmitForm()
	{
		await form.Validate();
		if (!form.IsValid)
		{
			return;
		}
		var model = _visit.ToModel();
		_btnPressed = true;
		var response = await VisitService.Add(model);

		if (response.IsSuccess)
		{
			await OnVisitCreated.InvokeAsync(response.Value!);
			MudDialog.Close();
		}
		_btnPressed = false;

	}
	public IMask postalCodeMask = new PatternMask("00-000");

	private void Cancel() => MudDialog.Cancel();
}
