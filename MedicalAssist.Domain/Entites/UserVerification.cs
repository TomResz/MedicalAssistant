using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class UserVerification
{
	public UserVerificationId Id { get; private set; }
	public UserId UserId { get; private set; }
	public CodeHash CodeHash { get; private set; }
	public Date ExpirationDate { get; private set; }

	private UserVerification() { }
	internal UserVerification(UserVerificationId id, UserId userId, Date expirationDate, CodeHash codeHash)
	{
		Id = id;
		UserId = userId;
		ExpirationDate = expirationDate;
		CodeHash = codeHash;
	}

	public void Regenerate(Date newExpirationDate, CodeHash newCodeHash)
	{
		if (ExpirationDate >= newExpirationDate)
		{
			throw new InvalidNewExpirationDate();
		}
		if(CodeHash.Equals(newCodeHash))
		{
			throw new SameCodeHashesException();
		}
		ExpirationDate = newExpirationDate;
		CodeHash = newCodeHash;
	}

}
