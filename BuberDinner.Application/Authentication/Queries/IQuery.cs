
using MediatR;

namespace BuberDinner.Application.Authentication.Queries;

public interface IQuery<TResponse> : IRequest<TResponse>
{
    
}