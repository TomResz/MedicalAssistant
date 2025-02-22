﻿using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;
public class UserVerification
{
	public UserId UserId { get; private set; }
	public CodeHash CodeHash { get; private set; }
	public Date ExpirationDate { get; private set; }

	private UserVerification() { }
	internal UserVerification(UserId userId, Date expirationDate, CodeHash codeHash)
	{
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
