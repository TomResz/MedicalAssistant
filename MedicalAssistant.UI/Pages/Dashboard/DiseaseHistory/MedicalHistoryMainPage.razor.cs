using MedicalAssistant.UI.Components.MedicalHistory;
using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.DiseaseHistory;

public partial class MedicalHistoryMainPage
{
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] public ILocalTimeProvider LocalTimeProvider { get; set; }
    public string SearchTerm { get; set; }
    private List<MedicalHistoryDto> _histories = new();
    private bool _loading = true;
    
    protected override async Task OnInitializedAsync()
    {
        var response = await MedicalHistoryService.GetAll();

        if (response.IsSuccess)
        {
            _histories = response.Value!;
            _loading = false;
        }
    }

    private async Task SearchDisease()
    {
        if (string.IsNullOrEmpty(SearchTerm))
        {
            var response = await MedicalHistoryService.GetAll();

            if (response.IsSuccess)
            {
                _histories = response.Value!;
                _loading = false;
            }

            await InvokeAsync(StateHasChanged);
            return;
        }

        var result = await MedicalHistoryService.GetByTerms(SearchTerm);

        if (result.IsSuccess)
        {
            _histories = result.Value!;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task Add()
    {
        var currentDate = await LocalTimeProvider.CurrentDate();
        var dialogParameters = new DialogParameters
        {
            { nameof(AddMedicalHistoryDialog.Date), currentDate}
        };

        var options = new MudBlazor.DialogOptions()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };
        var dialog = await DialogService.ShowAsync<AddMedicalHistoryDialog>(
            Translations.Add,
            dialogParameters, 
            options);

        var result = await dialog.Result;
        
        if (result is { Canceled: false, Data: MedicalHistoryDto dto })
        {
            _histories.Add(dto);
            _histories = _histories.OrderBy(x=>x.StartDate).ToList();
            await InvokeAsync(StateHasChanged);
        }
    }


    private void Edit(MedicalHistoryDto item) => NavigationManager.NavigateTo($"/medical-history/{item.Id}");

    private async Task Delete(Guid id)
    {
        Response response = await MedicalHistoryService.Delete(id);

        if (response.IsFailure)
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
            return;
        }

        var index = _histories.FindIndex(x => x.Id == id);

        if (index is not -1)
        {
            _histories.RemoveAt(index);
            await InvokeAsync(StateHasChanged);
        }
    }
}