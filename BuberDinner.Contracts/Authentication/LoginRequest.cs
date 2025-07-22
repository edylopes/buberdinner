namespace BuberDinner.Contracts.Authentication;

public record LoginRequest
{
    [Required(ErrorMessage = "Email is required ")]
    [EmailAddress(ErrorMessage = "Given email is no valid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required ")]
    public string Password { get; set; } = string.Empty;
}
