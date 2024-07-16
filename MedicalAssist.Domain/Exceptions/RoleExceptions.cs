﻿using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidRoleException : BadRequestException
{
	public InvalidRoleException(string role) : base($"Role: {role} is invalid.")
	{
	}
}
