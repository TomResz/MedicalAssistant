using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entites;
public class Recommendation
{
    public RecommendationId Id { get; private set; }
    public VisitId VisitId { get; private set; }
    public Note ExtraNote { get; private set; }
    public Date CreatedAt { get; private set; }
    public Medicine Medicine { get; private set; }
    public Date StartDate { get; private set; }
    public Date EndDate { get; private set; }
    private Recommendation() { }
    private Recommendation(
		RecommendationId id,
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

    public static Recommendation Create(
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

        Recommendation recommendation = new Recommendation(
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
