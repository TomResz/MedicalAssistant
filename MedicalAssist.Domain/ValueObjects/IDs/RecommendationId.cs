using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Exceptions.IDs;

namespace MedicalAssist.Domain.ValueObjects.IDs;
public sealed record RecommendationId
{
    public Guid Value { get;}
    public RecommendationId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidRecommendationIdException();
        }
        Value = value;
    }
    public static implicit operator Guid(RecommendationId id) => id.Value;
    public static implicit operator RecommendationId(Guid value) => new(value);
}
