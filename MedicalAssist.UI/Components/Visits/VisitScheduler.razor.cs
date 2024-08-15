using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace MedicalAssist.UI.Components.Visits;

public partial class VisitScheduler
{
	[Inject]
	public IVisitService VisitService { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }


	private List<VisitDto> _visits = new();
	private bool _loading = true;
	RadzenScheduler<VisitDto> _scheduler;
	protected override async Task OnInitializedAsync()
	{
		var response = await VisitService.GetAllVisits();
		if (response.IsSuccess)
		{
			_visits = response.Value!;
			await InvokeAsync(StateHasChanged);

		}
		_loading = false;
	}

	private void SelectTodayDate(SchedulerSlotRenderEventArgs args)
	{
		string[] validViews = { Translations.Month, Translations.Year };
		if (validViews.Contains(args.View.Text) && args.Start.Date == DateTime.Today)
		{
			args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
		}
	}

	private async Task AddVist(SchedulerSlotSelectEventArgs args)
	{
		var parameters = new MudBlazor.DialogParameters
		{
			{ "SelectedDate", args.Start },
			{ nameof(CreateVisitDialog.OnVisitCreated), EventCallback.Factory.Create<VisitDto>(this, OnVisitCreated) }
		};
		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };
		var dialog = DialogService.Show<CreateVisitDialog>("Add Visit", parameters);
		var result = await dialog.Result;
	}

	private async Task EditRemoveVisit(SchedulerAppointmentSelectEventArgs<VisitDto> args)
	{
		var parameters = new MudBlazor.DialogParameters
		{
			{ nameof(VisitDto), args.Data },
			{ nameof(EditRemoveVisitDialog.OnVisitDeleted), EventCallback.Factory.Create<Guid>(this, OnVisitDeleted) },
			{ nameof(EditRemoveVisitDialog.OnVisitEdited), EventCallback.Factory.Create<VisitDto>(this, OnVisitEdited) }
		};
		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true};
		var dialog = DialogService.Show<EditRemoveVisitDialog>(Translations.Visit, parameters);
		var result = await dialog.Result;
	}


	private async Task OnVisitDeleted(Guid visitId)
	{
		var visit = _visits.Where(x => x.Id == visitId).First();
		_visits.Remove(visit);
		await _scheduler.Reload();
	}

	private async Task OnVisitEdited(VisitDto visitDto)
	{
		var index = _visits.FindIndex(x => x.Id == visitDto.Id);
		if (index is not -1)
		{
			_visits[index] = visitDto;
			await _scheduler.Reload();
		}
	}

	private async Task OnVisitCreated(VisitDto visitModel)
	{
		_visits.Add(visitModel);
		await _scheduler.Reload();
	}
}
