namespace BuberDinner.Application.Common.Dto.Email.Enums
{
    public enum EmailConfirmationError
    {
        InvalidToken,
        ExpiredToken,
        UserNotFound,
        EmailAlreadyConfirmed

    }
}