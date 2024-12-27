using MediatR;

namespace MedicalAssistant.Application.Abstraction;
public interface IQuery<out TResponse> : IBaseQuery, IRequest<TResponse>
{
}
