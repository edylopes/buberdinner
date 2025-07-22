namespace BuberDinner.Domain.Exceptions;

public class UserRoleNotAllowedException : Exception
{
    public UserRoleNotAllowedException(string role)
        : base($"The user role '{role}' is not allowed.") { }
}
