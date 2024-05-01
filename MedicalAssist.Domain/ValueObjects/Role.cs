using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record Role
{
    public static IEnumerable<string> AvailableRoles { get; } = new[] { "admin", "user" };

    public string Value { get; }
    public Role(string value)
    {
        if (!AvailableRoles.Contains(value))
        {
            throw new InvalidRoleException(value);
        }
        Value = value;
    }
    public static Role Admin() => new Role("admin");
    public static Role User() => new Role("user");

    public static implicit operator Role(string value) => new Role(value);
    public static implicit operator string(Role role) => role.Value;

    public override string ToString() => Value;
}
