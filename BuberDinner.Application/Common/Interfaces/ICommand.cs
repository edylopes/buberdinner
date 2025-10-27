
namespace BuberDinner.Application.Common.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    string Operation { get; }
    string? Email { get; }
}
