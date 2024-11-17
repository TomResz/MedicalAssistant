using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class StagesTimeLine
{
    [Parameter] public Guid MedicalHistoryId { get; set; }
    [Parameter] public List<DiseaseStageDto>? Stages { get; set; }
    [Inject] public IDialogService DialogService { get; set; }

    private List<(DiseaseStageDto Stage, int Index)> _tuples = new();

    protected override void OnParametersSet()
    {
        InitData();
    }

    private void InitData()
    {
        if (Stages is null)
        {
            return;
        }

        _tuples.Clear();

        for (int i = 0; i < Stages.Count; i++)
        {
            _tuples.Add((Stages[i], i));
        }
    }

    private async Task Add()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(AddStageDialog.MedicalHistoryId), MedicalHistoryId }
        };

        var options = new MudBlazor.DialogOptions()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        };
        var dialog = await DialogService.ShowAsync<AddStageDialog>(
            Translations.AddingDiseaseStage,
            dialogParameters,
            options);

        var result = await dialog.Result;

        if (result is { Canceled: false, Data: DiseaseStageDto dto })
        {
            Stages ??= [];

            Stages.Add(dto);
            Stages = Stages.OrderBy(x => x.Date).ToList();
            InitData();
            await InvokeAsync(StateHasChanged);
        }
    }
}