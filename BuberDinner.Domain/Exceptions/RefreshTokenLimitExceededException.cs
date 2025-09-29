

namespace BuberDinner.Domain.Exceptions;

public class RefreshTokenLimitExceededException : DomainException
{
    public RefreshTokenLimitExceededException(string message)
    : base(message)
    {

    }
}
