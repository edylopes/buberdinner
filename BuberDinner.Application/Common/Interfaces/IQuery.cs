
using MediatR;

namespace BuberDinner.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}