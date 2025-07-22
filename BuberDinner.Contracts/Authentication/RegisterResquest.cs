using System.ComponentModel.DataAnnotations;

namespace BuberDinner.Contracts.Authentication;

public record RegisterRequest
{
    [Required(ErrorMessage = "Name is required ")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required ")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required ")]
    [EmailAddress(ErrorMessage = "Given email is not valid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required ")]
    public string Password { get; set; } = string.Empty;
}
