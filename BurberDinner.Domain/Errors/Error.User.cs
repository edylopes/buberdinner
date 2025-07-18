using ErrorOr;

namespace BurberDinner.DomainErros;


/// <summary>
/// Contains error definitions related to user operations.
/// </summary>
public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use.");
    }
}