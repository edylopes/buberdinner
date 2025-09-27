using System.ComponentModel.DataAnnotations;

namespace BuberDinner.Contracts.Authentication;

public record RegisterRequest
{
    [Required(ErrorMessage = "Name is required ")]
    [MinLength(3, ErrorMessage = "First name must be at least 2 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required ")]
    [MinLength(3, ErrorMessage = "Last name must be at least 2 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required ")]
    [EmailAddress(ErrorMessage = "Given email is not valid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required ")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;
}
