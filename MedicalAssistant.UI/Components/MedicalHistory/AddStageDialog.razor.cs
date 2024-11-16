﻿using FluentValidation;
using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class AddStageDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
    [Inject] public IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    [Parameter] public Guid MedicalHistoryId { get; set; }
    
    private MudForm _form;
    private readonly DiseaseStageViewModel _viewModel = new();
    private readonly DiseaseStageViewModelValidator _validator = new();

    private VisitDto? _visitDto = null;


    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        var request = new AddDiseaseStageRequest()
        {
            Name = _viewModel.Name,
            Date = _viewModel.Date!.Value,
            VisitId = _viewModel.VisitId,
            Note = _viewModel.Notes,
        };
        
        Response<Guid> response = await MedicalHistoryService.AddStage(request, MedicalHistoryId);

        if (response.IsSuccess)
        {
            var id = response.Value!;
            DiseaseStageDto dto = new()
            {
                MedicalHistoryId = MedicalHistoryId,
                Date = request.Date,
                VisitDto = _visitDto,
                Id = id,
                Name = _viewModel.Name,
                Note = _viewModel.Notes,
            };
            
            MudDialog.Close(DialogResult.Ok(dto));
        }
        else
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
        }
    }

    private async Task VisitPick(VisitDto visitDto)
    {
        _visitDto = visitDto;
        _viewModel.VisitId = visitDto.Id;
        await InvokeAsync(StateHasChanged);
    }

    private async Task Delete()
    {
        _visitDto = null;
        _viewModel.VisitId = null;
        await InvokeAsync(StateHasChanged);
    }

    private void Cancel() => MudDialog.Cancel();
}

public class AddDiseaseStageRequest
{
    public Guid? VisitId { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
}
public class DiseaseStageViewModel
{
    public Guid? VisitId { get; set; }
    public string Name { get; set; }
    public string? Notes { get; set; }
    public DateTime? Date { get; set; }
}

public class DiseaseStageViewModelValidator : BaseValidator<DiseaseStageViewModel>
{
    public DiseaseStageViewModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage(Translations.EmptyField)
            .NotEmpty()
            .WithMessage(Translations.EmptyField)
            .MaximumLength(30)
            .WithMessage(Translations.ExceededMaxSizeOfField);
    }
}