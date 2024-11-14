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
    public string SearchTerm { get; set; }


    private List<MedicalHistoryDto> _histories = new();
    private bool _loading = true;

    [Inject] public IMedicalHistoryService MedicalHistoryService { get; set; }

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
        // TO DO
        // FETCHING FILTERED DATA
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
        
        var index = _histories.FindIndex(x=>x.Id == id);

        if (index is not -1)
        {
            _histories.RemoveAt(index);
            await InvokeAsync(StateHasChanged);
        }
    }
}