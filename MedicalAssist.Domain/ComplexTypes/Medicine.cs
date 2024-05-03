using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Domain.ComplexTypes;
public class Medicine
{
    public MedicineName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public TimeOfDay TimeOfDay { get; private set; }

    public Medicine(MedicineName name, Quantity quantity, TimeOfDay timeOfDay)
	{
		Name = name;
		Quantity = quantity;
		TimeOfDay = timeOfDay;
	}
}
