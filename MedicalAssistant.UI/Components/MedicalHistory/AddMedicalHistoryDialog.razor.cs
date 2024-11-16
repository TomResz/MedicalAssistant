using System.Data;
using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
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

public partial class AddMedicalHistoryDialog : ComponentBase
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; }

    [Parameter] public DateTime Date { get; set; }
    [Inject] public IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }

    private MudForm _form;
    private readonly MedicalHistoryViewModel _viewModel = new();
    private readonly MedicalHistoryViewModelValidator _validator = new();

    private VisitDto? _visitDto = null;

    protected override void OnParametersSet()
    {
        _viewModel.StartDate = Date;
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        Response<Guid> response = await MedicalHistoryService.Add(_viewModel);

        if (response.IsFailure)
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Success);
            return;
        }

        var id = response.Value!;

        var dto = new MedicalHistoryDto()
        {
            Id = id,
            VisitDto = _visitDto,
            StartDate = Date,
            EndDate = null,
            Notes = _viewModel.Notes,
            SymptomDescription = _viewModel.SymptomDescription,
            Stages = [],
            DiseaseName = _viewModel.Name
        };

        MudDialog.Close(DialogResult.Ok(dto));
        Snackbar.Add(Translations.MedicalHistoryAdded, Severity.Success);
    }

    private void VisitPick(VisitDto visitDto)
    {
        _visitDto = visitDto;
        _viewModel.VisitId = visitDto.Id;
        StateHasChanged();
    }

    private void Delete()
    {
        _visitDto = null;
        _viewModel.VisitId = null;
        StateHasChanged();
    }

    private void Cancel() => MudDialog.Cancel();
}

public class MedicalHistoryViewModel
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Notes { get; set; }
    public string? SymptomDescription { get; set; }
    public Guid? VisitId { get; set; }
}

public class MedicalHistoryViewModelValidator : BaseValidator<MedicalHistoryViewModel>
{
    public MedicalHistoryViewModelValidator()
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