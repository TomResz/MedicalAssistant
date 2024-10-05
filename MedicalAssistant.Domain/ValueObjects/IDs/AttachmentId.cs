using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;

public sealed record AttachmentId
{
    public Guid Value { get; }

    public AttachmentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidAttachmentIdException();
        }
        Value = value;
    }
    public static implicit operator AttachmentId(Guid value) => new(value);
    public static implicit operator Guid(AttachmentId value) => value.Value;    
}
