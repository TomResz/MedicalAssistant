namespace MedicalAssist.Infrastructure.Docker;
internal sealed class DockerChecker : IDockerChecker
{
	public bool IsRunningInContainer => Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER") == "true";
}
