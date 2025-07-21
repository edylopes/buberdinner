using System.ComponentModel.DataAnnotations;

namespace BurberDinner.Contracts.Authentication;

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email não é válido")]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
