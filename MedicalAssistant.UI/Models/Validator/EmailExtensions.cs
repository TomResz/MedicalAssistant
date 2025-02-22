﻿using FluentValidation;
using MedicalAssistant.UI.Shared.Resources;
using System.Text.RegularExpressions;

namespace MedicalAssistant.UI.Models.Validator;

public static class EmailExtensions
{
	private static readonly Regex Regex = new(
   @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
   RegexOptions.Compiled);
	public static IRuleBuilder<T, string> EmailMustBeValid<T>(this IRuleBuilder<T, string> ruleBuilder)
		=> ruleBuilder
			.NotEmpty()
			 .WithMessage(Translations.Empty_Email)
			 .Must(Regex.IsMatch)
			 .WithMessage(Translations.EmailInvalidPattern);
}
