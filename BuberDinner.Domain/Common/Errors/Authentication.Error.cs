
namespace BuberDinner.Domain.Common.Errors;

public record InvalidCredentialError()
    : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl , "Invalid email or password");