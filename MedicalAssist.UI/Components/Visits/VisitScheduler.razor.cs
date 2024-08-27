using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace MedicalAssist.UI.Components.Visits;

public partial class VisitScheduler
{
	[Parameter]
	public IReadOnlyList<VisitDto> Visits { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }


	RadzenScheduler<VisitDto> _scheduler;

	private List<VisitDto> _visits = [];


	protected override async Task OnInitializedAsync()
	{
		_visits = [.. Visits];
		await InvokeAsync(StateHasChanged);
	}

	private void SelectTodayDate(SchedulerSlotRenderEventArgs args)
	{
		string[] validViews = [Translations.Month, Translations.Year];
		if (validViews.Contains(args.View.Text) && args.Start.Date == DateTime.Today)
		{
			args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
		}
	}

	private async Task AddVist(SchedulerSlotSelectEventArgs args)
	{
		string[] validViews = [Translations.Month, Translations.Day, Translations.Week];

		if (!validViews.Contains(args.View.Text))
		{
			return;
		}

		var parameters = new DialogParameters
		{
			{ "SelectedDate", args.Start },
			{ nameof(CreateVisitDialog.OnVisitCreated), EventCallback.Factory.Create<VisitDto>(this, OnVisitCreated) }
		};
		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };
		var dialog = DialogService.Show<CreateVisitDialog>(Translations.AddVisitTitle, parameters);
		var result = await dialog.Result;
	}

	private async Task EditRemoveVisit(SchedulerAppointmentSelectEventArgs<VisitDto> args)
	{
		var parameters = new DialogParameters
		{
			{ nameof(VisitDto), args.Data },
			{ nameof(EditRemoveVisitDialog.OnVisitDeleted), EventCallback.Factory.Create<Guid>(this, OnVisitDeleted) },
			{ nameof(EditRemoveVisitDialog.OnVisitEdited), EventCallback.Factory.Create<VisitDto>(this, OnVisitEdited) }
		};
		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };
		var dialog = DialogService.Show<EditRemoveVisitDialog>(Translations.Visit, parameters);
		var result = await dialog.Result;
	}


	private async Task OnVisitDeleted(Guid visitId)
	{
		var visit = _visits.Where(x => x.Id == visitId).FirstOrDefault();
		if (visit is not null)
		{
			_visits.Remove(visit);
			await _scheduler.Reload();
		}
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
