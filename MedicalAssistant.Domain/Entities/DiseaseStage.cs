using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;

public class DiseaseStage
{
    public DiseaseStageId Id { get; private set; }
    public DiseaseStageName Name { get; private set; }
    public Date Date { get; private set; }
    public Note Note { get; private set; }
    public MedicalHistoryId MedicalHistoryId { get; private set; }
    public VisitId? VisitId { get; private set; }

    private DiseaseStage()
    {
    }

    internal DiseaseStage(
        DiseaseStageId id,
        DiseaseStageName name,
        Date date,
        Note note,
        MedicalHistoryId medicalHistoryId,
        VisitId? visitId)
    {
        Id = id;
        Name = name;
        Date = date;
        Note = note;
        MedicalHistoryId = medicalHistoryId;
        VisitId = visitId;
    }
}