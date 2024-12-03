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
        if (DiseaseEndDate is not null)
        {
            throw new CannotModifyCompletedDiseaseHistoryException();
        }
        if (stage.Date.ToDate() <= DiseaseStartDate.ToDate())
        {
            throw new InvalidStageDateException();
        }
        _diseaseStages.Add(stage);
    }

    public void Edit(
        DiseaseName name,
        Date startDate,
        Date? endDate,
        Note symptomDescription,
        Note notes,
        VisitId? visitId)
    {
        if (DiseaseEndDate is not null)
        {
            throw new CannotModifyCompletedDiseaseHistoryException();
        }
        
        if (_diseaseStages.Count != 0 )
        {
            var date = _diseaseStages
                .Select(x=>x.Date.Value.Date)
                .Order()
                .ToList();
            
            var beginDate = date.Min();
            var lastDate = date.Max();
            
            if (startDate.ToDate() > beginDate)
            {
                throw new InvalidStartDateException();
            }

            if (endDate is not null && endDate.ToDate() < lastDate)
            {
                throw new InvalidEndDateException();
            }
        }

        DiseaseName = name;
        DiseaseStartDate = startDate;
        DiseaseEndDate = endDate;
        SymptomDescription = symptomDescription;
        Notes = notes;
        VisitId = visitId;
    }


    public void DeleteStage(DiseaseStageId stageId)
    {
        var stage = _diseaseStages.FirstOrDefault(x => x.Id == stageId);

        if (stage is null)
        {
            throw new UnknownDiseaseStageException(stageId);
        }

        _diseaseStages.Remove(stage);
    }

    public void EditStage(DiseaseStage stage, DiseaseStageName requestName, Note requestNote, VisitId? requestVisitId, Date requestDate)
    {
        if (DiseaseStartDate.ToDate() > requestDate.ToDate() ||
            DiseaseEndDate?.ToDate() < requestDate.ToDate())
        {
            throw new InvalidStageDateException();
        }
        stage.Edit(requestName, requestNote, requestVisitId, requestDate);
    }
}