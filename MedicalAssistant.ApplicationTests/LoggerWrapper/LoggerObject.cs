using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.Tests.LoggerWrapper;
public class LoggerObject<T> : ILogger<T>
{
	ILogger<T> _log;
	public LoggerObject(ILogger<T> log) => _log = log;
	public IDisposable BeginScope<TState>(TState state) 
		=> _log.BeginScope<object>(state);
	public bool IsEnabled(LogLevel logLevel) 
		=> _log.IsEnabled(logLevel);
	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		_log.Log<object>(logLevel, eventId, state, exception, (st, ex) => "");
	}
}
