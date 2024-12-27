using MediatR;

namespace MedicalAssistant.Application.Abstraction;

public interface IQueryHandler<in TQuery, TResponse> : IBaseQuery, IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}