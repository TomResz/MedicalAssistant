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

	private MudForm form;
	private VisitViewModel visitForm = new();

	protected override void OnInitialized()
	{
		visitForm.Date = SelectedDate;
	}

	private async Task SubmitForm()
	{
		if (!form.IsValid)
		{
			return;
		}
		var model = visitForm.ToModel();

		var response = await VisitService.Add(model);

		if(response.IsSuccess)
		{
			await OnVisitCreated.InvokeAsync(response.Value!);
			MudDialog.Close();
		}

	}
	public IMask postalCodeMask = new PatternMask("00-000");

	private void Cancel() => MudDialog.Cancel();
}
