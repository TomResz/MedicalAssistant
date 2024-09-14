namespace MedicalAssist.Infrastructure.Docker;

public interface IDockerChecker
{
	bool IsRunningInContainer { get; }
}