using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.ValueObjects;
using System.Reflection;

namespace MedicalAssistant.Application.Tests.ObjectFactories;
public static class UserFactory
{
	public static Domain.Entities.User CreateUser(
		string email = "example@gmail.com",
		string password = "notSecuredPassword")
	{
		var user = Domain.Entities.User.Create(
	email,
	password,
	"John Done",
	Role.User(),
	DateTime.UtcNow,
	"codeHash",
	Languages.Polish);
		var isVerifiedField = user.GetType().GetField("<IsVerified>k__BackingField",
			BindingFlags.Instance | BindingFlags.NonPublic);
		isVerifiedField?.SetValue(user, true);

		return user;
	}


	public static Domain.Entities.User CreateNotVerifiedUser(
		string email = "example@gmail.com",
		string password = "notSecuredPassword")
	{
		var user = Domain.Entities.User.Create(
	email,
	password,
	"John Done",
	Role.User(),
	DateTime.UtcNow,
	"codeHash",
	Languages.Polish);
		var isVerifiedField = user.GetType().GetField("<IsVerified>k__BackingField",
			BindingFlags.Instance | BindingFlags.NonPublic);
		isVerifiedField?.SetValue(user, false);

		return user;
	}

	public static Domain.Entities.User CreateWithExternalAuthProvider(
		string email = "example@gmail.com",
		string provider = "google",
		string key = "12345678")
	{
		var user = Domain.Entities.User.CreateByExternalProvider(
	email,
	"John Done",
	Role.User(),
	DateTime.UtcNow,
	key,
	 provider,
		   Languages.English);

		return user;
	}
}
