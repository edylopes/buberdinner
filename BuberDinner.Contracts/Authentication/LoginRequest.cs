using System.ComponentModel.DataAnnotations;

namespace BuberDinner.Contracts.Authentication;

public record LoginRequest(string email, string password);