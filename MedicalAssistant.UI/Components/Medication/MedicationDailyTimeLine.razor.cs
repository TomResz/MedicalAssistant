using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.Medication;

public partial class MedicationDailyTimeLine
{
    
    [Inject] public IMedicationService MedicationService { get; set; }

    [Inject] public ILocalTimeProvider LocalTimeProvider { get; set; }


    private List<MedicationDto> _medications = new();

    private List<MedicationDto> _morningMedications = new();

    private List<MedicationDto> _afternoonMedications = new();

    private List<MedicationDto> _eveningMedications = new();

    private List<MedicationDto> _nightMedications = new();

    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        var currentDate = (await LocalTimeProvider.CurrentDate()).Date;
        var response = await MedicationService.GetByDate(currentDate);

        if (response.IsSuccess)
        {
            _medications = response.Value!;

            _morningMedications = _medications
                .Where(x => x.TimeOfDay.Contains(MedicationMappers.Morning))
                .ToList();
            _afternoonMedications = _medications
                .Where(x => x.TimeOfDay.Contains(MedicationMappers.Afternoon))
                .ToList();
            _eveningMedications = _medications
                .Where(x => x.TimeOfDay.Contains(MedicationMappers.Evening))
                .ToList();
            _nightMedications = _medications
                .Where(x => x.TimeOfDay.Contains(MedicationMappers.Night))
                .ToList();
        }

        _loading = false;
    }

}