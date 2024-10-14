using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record MedicationRecommendationId
{
    public Guid Value { get;}
    public MedicationRecommendationId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidRecommendationIdException();
        }
        Value = value;
    }
    public static implicit operator Guid(MedicationRecommendationId id) => id.Value;
    public static implicit operator MedicationRecommendationId(Guid value) => new(value);
}
