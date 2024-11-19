using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class EditMedicalHistoryDialog
{
    [CascadingParameter] public MudDialogInstance DialogInstance { get; set; }
    [Inject] private IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Parameter] public MedicalHistoryDto MedicalHistory { get; set; }
    
    private bool _editing = false;
    
    private readonly EditMedicalHistoryViewModel _viewModel = new();
    private readonly EditMedicalHistoryViewModelValidator _validator = new();
    private MudForm _form;
    private VisitDto? _visitDto = null;

    protected override void OnParametersSet()
    {
        _visitDto = MedicalHistory.VisitDto;
        _viewModel.StartDate = MedicalHistory.StartDate;
        _viewModel.EndDate = MedicalHistory.EndDate;
        _viewModel.VisitId = MedicalHistory.VisitDto?.Id;
        _viewModel.Name = MedicalHistory.DiseaseName;
        _viewModel.SymptomDescription = MedicalHistory.SymptomDescription;
        _viewModel.Notes = MedicalHistory.Notes;
    }

    private async Task Edit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }
        _editing = true;
        
        var request = new EditMedicalHistoryRequest()
        {
            StartDate = MedicalHistory.StartDate,
            SymptomDescription = _viewModel.SymptomDescription,
            Id = MedicalHistory.Id,
            Name = _viewModel.Name,
            Notes = _viewModel.Notes,
            EndDate = _viewModel.EndDate,
            VisitId = _viewModel.VisitId,
        };

        var response = await MedicalHistoryService.Update(request);

        if (response.IsSuccess)
        {
            var dto = new MedicalHistoryDto()
            {
                Stages = MedicalHistory.Stages,
                DiseaseName = request.Name,
                Notes = request.Notes,
                EndDate = request.EndDate,
                SymptomDescription = request.SymptomDescription,
                Id = MedicalHistory.Id,
                StartDate = (DateTime)request.StartDate!,
                VisitDto = _visitDto,
            };

            DialogInstance.Close(dto);
            Snackbar.Add(Translations.SuccessfullyEdited, Severity.Success);
            return;
        }

        var errorType = response.ErrorDetails!.Type switch
        {
            // TO DO
            _ => Translations.SomethingWentWrong
        };
        Snackbar.Add(errorType, Severity.Error);
        _editing = false;
    }

    private void Cancel() => DialogInstance.Cancel();

    private void VisitPick(VisitDto visitDto)
    {
        (_visitDto,_viewModel.VisitId,_viewModel.StartDate) = (visitDto,visitDto.Id,visitDto.Date);
        StateHasChanged();
    }

    private void Delete()
    {
        (_visitDto,_viewModel.VisitId) = (null,null);
        StateHasChanged();
    }
}