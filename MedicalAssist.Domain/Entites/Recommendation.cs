using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class Recommendation
{
    public RecommendationId Id { get; private set; }
    public Note ExtraNote { get; private set; }
    public Date CreatedAt { get; private set; }
    public Medicine Medicine { get; private set; }
    private Recommendation() { }
    private Recommendation(RecommendationId id,Note extraNote,Date createdAt,Medicine medicine)
    {
        Id = id;
        ExtraNote = extraNote;
        CreatedAt = createdAt;
        Medicine = medicine;
    }

    public static Recommendation Create(Note extraNote, Date createdAt, Medicine medicine)
    {
        Recommendation recommendation = new Recommendation(
            Guid.NewGuid(),
            extraNote,
            createdAt,
            medicine);
        return recommendation;
    }
}
