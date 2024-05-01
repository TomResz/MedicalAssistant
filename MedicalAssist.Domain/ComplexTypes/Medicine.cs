using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Domain.ComplexTypes;
public class Medicine
{
    public MedicineName Name { get; private set; }
    public Quantity Quantity { get; private set; }

    public Medicine(MedicineName name, Quantity quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}
