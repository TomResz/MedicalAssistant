using MediatR;

namespace MedicalAssistant.Application.Abstraction;
public interface ICommand<out TResponse> : IBaseCommand, IRequest<TResponse>
{
}

public interface ICommand : IBaseCommand, IRequest
{
}