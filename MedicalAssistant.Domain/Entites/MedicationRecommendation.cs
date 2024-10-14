using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entites;
public class MedicationRecommendation
{
    public MedicationRecommendationId Id { get; private set; }
    public VisitId? VisitId { get; private set; }
    public UserId UserId { get; set; }
    public Note ExtraNote { get; private set; }
    public Date CreatedAt { get; private set; }
    public Medicine Medicine { get; private set; }
    public Date StartDate { get; private set; }
    public Date EndDate { get; private set; }
    public User User { get;private set; }
    private MedicationRecommendation() { }
    private MedicationRecommendation(
		MedicationRecommendationId id,
		VisitId visitId,
		Note extraNote,
		Date createdAt,
		Medicine medicine,
		Date startDate,
		Date endDate)
    {
        Id = id;
        VisitId = visitId;
        ExtraNote = extraNote;
        CreatedAt = createdAt;
        Medicine = medicine;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static MedicationRecommendation Create(
		VisitId visitId,
		Note extraNote,
		Date createdAt,
		Medicine medicine,
		Date startDate,
		Date endDate)
    {
        if (startDate > endDate)
        {
            throw new InvalidEndDateException();
        }

        MedicationRecommendation recommendation = new MedicationRecommendation(
            Guid.NewGuid(),
			visitId,
			extraNote,
            createdAt,
            medicine,
            startDate, 
            endDate);

        return recommendation;
    }
}
