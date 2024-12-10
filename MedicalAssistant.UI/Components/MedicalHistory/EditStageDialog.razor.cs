using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class EditStageDialog 
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
    [Parameter] public DiseaseStageDto DiseaseStage { get; set; }
    
    [Inject] private IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    
    private MudForm _form;
    private readonly EditStageViewModel _viewModel = new();
    private readonly EditStageViewModelValidator _validator = new();
    private VisitDto? _visitDto = null;

    protected override void OnParametersSet()
    {
        _visitDto = DiseaseStage.VisitDto;
        _viewModel.Date = DiseaseStage.Date;
        _viewModel.MedicalHistoryId = DiseaseStage.MedicalHistoryId;
        _viewModel.Name = DiseaseStage.Name;
        _viewModel.Note = DiseaseStage.Note;
        _viewModel.VisitId = DiseaseStage.VisitDto?.Id;
        
    }

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid)
        {
            return;
        }

        var request = new EditDiseaseStageRequest
        {
            Id = DiseaseStage.Id,
            Name = _viewModel.Name,
            Note = _viewModel.Note,
            VisitId = _viewModel.VisitId,
            Date = (DateTime)_viewModel.Date!
        };

        Response response = await MedicalHistoryService.EditStage(request, DiseaseStage.MedicalHistoryId);

        if (response.IsFailure)
        {
            string errorMessage = response.ErrorDetails!.Type switch
            {
                "MedicalHistoryIsCompleted" => Translations.MedicalHistoryAlreadyCompletedMessage,
                _ => Translations.SomethingWentWrong
            };
            Snackbar.Add(errorMessage, Severity.Error);
            return;
        }

        var dto = new DiseaseStageDto()
        {
            MedicalHistoryId = DiseaseStage.MedicalHistoryId,
            Date = request.Date,
            VisitDto = _visitDto,
            Id = DiseaseStage.Id,
            Name = _viewModel.Name,
            Note = _viewModel.Note,
        };
        
        MudDialog.Close(dto);
        Snackbar.Add(Translations.SuccessfullyEdited, Severity.Success);
    }

    private void VisitPick(VisitDto visitDto)
    {
        _visitDto = visitDto;
        _viewModel.VisitId = visitDto.Id;
        _viewModel.Date = visitDto.Date;
        StateHasChanged();
    }

    private void DeleteVisit()
    {
        (_visitDto, _viewModel.VisitId) = (null,null);
        StateHasChanged();
    }

    private async Task Delete()
    {
        bool? result = await DialogService.ShowMessageBox(
            Translations.DialogDiseaseStageDelete,
            (MarkupString)Translations.DialogEditDiseaseStageRemove,
            yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

        if (result is null or false)
        {
            return;
        }

        var id = DiseaseStage.Id;
        Response response = await MedicalHistoryService.DeleteStage(id,DiseaseStage.MedicalHistoryId);

        if (response.IsSuccess)
        {
            MudDialog.Close(id);
            return;
        }
        Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
    }
    private void Cancel() => MudDialog.Cancel();
}