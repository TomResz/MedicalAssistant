using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;

public sealed record MedicalNoteId
{
    public Guid Value { get; init; }

    public MedicalNoteId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidMedicalNoteIdException();
        }

        Value = value;
    }

    public static implicit operator Guid(MedicalNoteId medicalNoteId) => medicalNoteId.Value;
    public static implicit operator MedicalNoteId(Guid value) => new(value);
}