using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record CodeHash
{
    public string Value { get; private set; }
    public CodeHash(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyCodeHashException();
        }
        Value = value;
    }

    public static implicit operator string(CodeHash codeHash) => codeHash.Value;
    public static implicit operator CodeHash(string value) => new(value);

}
