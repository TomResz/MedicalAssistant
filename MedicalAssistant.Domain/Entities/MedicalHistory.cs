using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;

public class MedicalHistory
{
    public MedicalHistoryId Id { get; private set; }
    public UserId UserId { get; private set; }
    public Date DiseaseStartDate { get; private set; }
    public Date? DiseaseEndDate { get; private set; }
    public DiseaseName DiseaseName { get; private set; }
    public Note Notes { get; private set; }
    public Note SymptomDescription { get; private set; }
    public VisitId? VisitId { get; private set; }
    
    // EF
    public Visit? Visit { get; private set; }
    public User User { get; private set; }

    private readonly HashSet<DiseaseStage> _diseaseStages = [];
    public IEnumerable<DiseaseStage> DiseaseStages => _diseaseStages;

    private MedicalHistory()
    {
    }

    private MedicalHistory(
        MedicalHistoryId id,
        UserId userId,
        Date diseaseStartDate,
        Date? diseaseEndDate,
        DiseaseName diseaseName,
        Note notes,
        Note symptomDescription,
        VisitId? visitId)
    {
        Id = id;
        UserId = userId;
        DiseaseStartDate = diseaseStartDate;
        DiseaseEndDate = diseaseEndDate;
        DiseaseName = diseaseName;
        Notes = notes;
        SymptomDescription = symptomDescription;
        VisitId = visitId;
    }


    public static MedicalHistory Create(
        UserId userId,
        Date diseaseStartDate,
        Date? diseaseEndDate,
        DiseaseName diseaseName,
        Note notes,
        Note symptomDescription,
        VisitId? visitId)
    {
        if (diseaseEndDate is not null &&
            diseaseStartDate < diseaseEndDate)
        {
            throw new InvalidEndDateException();
        }

        return new MedicalHistory(
            Guid.NewGuid(),
            userId,
            diseaseStartDate,
            diseaseEndDate,
            diseaseName,
            notes,
            symptomDescription,
            visitId);
    }

    public void AddStage(DiseaseStage stage)
    {
        if (stage.Date.ToDate() <= DiseaseStartDate.ToDate())
        {
            throw new InvalidStageDateException();
        }
        _diseaseStages.Add(stage);
    }
    
}