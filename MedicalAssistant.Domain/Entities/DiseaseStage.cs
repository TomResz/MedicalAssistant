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
    
    public Visit? Visit { get; private set; }
    private DiseaseStage()
    {
    }

    private DiseaseStage(
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


    public static DiseaseStage Create(
        VisitId? visitId,
        DiseaseStageName name,
        Date date,
        Note note,
        MedicalHistoryId medicalHistoryId)
    {
        return new(
            Guid.NewGuid(),
            name, 
            date,
            note,
            medicalHistoryId,
            visitId);
    }

    internal void Edit(DiseaseStageName requestName, Note requestNote, VisitId? requestVisitId, Date requestDate)
    {
        Name = requestName;
        Note = requestNote;
        Date = requestDate;
        VisitId = requestVisitId;
    }
}