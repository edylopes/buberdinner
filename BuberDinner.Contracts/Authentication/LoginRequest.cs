using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace BuberDinner.Contracts.Authentication;

public record LoginRequest(string Email, string Password);