namespace MedicalAssistant.Infrastructure.Docker;

public interface IDockerChecker
{
	bool IsRunningInContainer { get; }
}