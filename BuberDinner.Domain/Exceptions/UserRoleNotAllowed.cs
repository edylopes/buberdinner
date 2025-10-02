
namespace BuberDinner.Domain.Exceptions;

public class UserRoleNotAllowed : DomainException
{
    public UserRoleNotAllowed()
        : base("User Role not allowed")
    {
    }
}
