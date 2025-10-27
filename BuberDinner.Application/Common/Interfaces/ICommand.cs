
namespace BuberDinner.Application.Common.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
{

}