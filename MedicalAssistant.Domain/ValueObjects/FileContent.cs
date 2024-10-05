using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;

public sealed record FileContent
{
    public const int MaxSize = 10 * 1024 * 1024;

    public byte[] Value { get; }

    public FileContent(byte[] value)
    {
        if (value.Length == 0)
        {
            throw new EmptyFileContentException();
        }
        else if (value.Length > MaxSize)
        {
            throw new ExceededFileContentMaximumSizeException(MaxSize);
        }
        Value = value;
    }

    public static implicit operator byte[](FileContent value) => value.Value;
    public static implicit operator FileContent(byte[] value) => new(value);
}
