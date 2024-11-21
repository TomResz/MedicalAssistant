using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class StagesTimeLine
{
    [Parameter] public Guid MedicalHistoryId { get; set; }
    [Parameter] public List<DiseaseStageDto>? Stages { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    private List<(DiseaseStageDto Stage, int Index)> _tuples = [];

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

        Stages = Stages.OrderBy(x => x.Date).ToList();
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

    private async Task Edit(Guid stageId)
    {
        var stage = Stages?.FirstOrDefault(x => x.Id == stageId);

        if (stage is null)
        {
            return;
        }

        var parameters = new DialogParameters
        {
            { nameof(EditStageDialog.DiseaseStage), stage },
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large
        };

        var dialog = await DialogService.ShowAsync<EditStageDialog>(
            Translations.EditDiseaseStageDialogTitle,
            parameters,
            options);

        var response = await dialog.Result;

        if (response is null || response.Canceled)
        {
            return;
        }

        var data = response.Data;

        switch (data)
        {
            // DELETED
            case Guid id:
            {
                var temp = Stages!.First(x => x.Id == id);
                Stages!.Remove(temp);
                break;
            }
            // EDITED 
            case DiseaseStageDto dto:
            {
                var index = Stages!.FindIndex(x => x.Id == dto.Id);

                if (index is not -1)
                {
                    Stages[index] = dto;
                }

                break;
            }
        }

        InitData();
        await InvokeAsync(StateHasChanged);
    }
}