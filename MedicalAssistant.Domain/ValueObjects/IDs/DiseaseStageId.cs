using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;

public sealed record DiseaseStageId
{
    public Guid Value { get; }
    public DiseaseStageId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidDiseaseStageIdException();
        }

        Value = value;
    }

    public static implicit operator Guid(DiseaseStageId value) => value.Value;
    public static implicit operator DiseaseStageId(Guid value) => new(value);
}