using MedicalAssist.Domain.Exceptions;
using System.Drawing;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record Note
{
    public string? Value { get; }

    public Note(string? value = null)
    {
        if(value is not null && 
            value.Length is < 3 or  > 500)
        {
            throw new InvalidNoteLengthException();
        }
        Value = value;  
    }
    public static implicit operator string?(Note note) => note.Value;
    public static implicit operator Note(string? value) => new(value);  
}
