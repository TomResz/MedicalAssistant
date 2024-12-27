using MediatR;

namespace MedicalAssistant.Application.Abstraction;
public interface ICommandHandler<in TCommand>
    : IBaseCommand, IRequestHandler<TCommand>
        where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse>
    : IBaseCommand, IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
}