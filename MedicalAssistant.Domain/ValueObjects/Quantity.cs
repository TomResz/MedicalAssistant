using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record Quantity
{
    public int Value { get; private set; }

    public Quantity(int value)
    {
        if(value <= 0)
        {
            throw new InvalidQuantityException();
        }
        Value = value;
    }
    public static implicit operator int(Quantity value) => value.Value;
    public static implicit operator Quantity(int value) => new Quantity(value); 
}
