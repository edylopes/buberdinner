



namespace BuberDinner.Application.Common.Beahviors.Logger;

public interface ILoggingStrategy<TRequest, TResponse>
{
  Task LogAsync(TRequest request, TResponse response);
}
