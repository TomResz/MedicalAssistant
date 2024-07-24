
namespace MedicalAssist.UI.Shared.Services.Verification;

public interface IUserVerificationService
{
	Task<bool> VerifyAccount(string code);
}