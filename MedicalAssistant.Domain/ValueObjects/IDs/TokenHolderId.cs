namespace MedicalAssistant.Domain.ValueObjects.IDs;

public sealed record TokenHolderId
{
	public Guid Value { get; }

	public TokenHolderId(Guid value)
	{
		if (value == Guid.Empty)
		{

		}
		Value = value;
	}

	public static implicit operator Guid(TokenHolderId tokenHolderId) => tokenHolderId.Value;
	public static implicit operator TokenHolderId(Guid guid) => new(guid);
}
