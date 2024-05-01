using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Domain.ComplexTypes;
public class Address
{
    public City City { get; private set; }
    public PostalCode PostalCode { get; private set; }
    public Street Street { get; private set; }

    public Address(Street street, City city, PostalCode postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }

}
