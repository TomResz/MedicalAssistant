﻿using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyEmailException : BadRequestException
{
	public EmptyEmailException() : base("The email provided is empty.")
	{
	}
}
public sealed class IncorrectEmailPatternException : BadRequestException
{
	public IncorrectEmailPatternException() : base("Incorrect email pattern.")
	{
	}
}
