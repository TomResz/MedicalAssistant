using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;

public sealed record MedicalHistoryId
{
    public Guid Value { get; init; }
    
    public MedicalHistoryId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidMedicalHistoryIdException();
        }
        Value = value;  
    }
    
    public static implicit operator Guid(MedicalHistoryId medicalHistoryId) => medicalHistoryId.Value;
    public static implicit operator MedicalHistoryId(Guid value) => new (value);
}