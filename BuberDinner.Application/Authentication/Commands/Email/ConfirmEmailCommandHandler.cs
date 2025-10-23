using BuberDinner.Application.Authentication.Common.Dto;
using BuberDinner.Application.Common.Dto;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Email;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, OneOf<EmailConfirmed, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<EmailConfirmed, AppError>> Handle(
        ConfirmEmailCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

        if (user is null)
            return new EmailConfirmationFailed();

        if (user.EmailConfirmed)
            return new EmailAlreadyConfirmed();

        user.ConfirmEmail();

        _userRepository.Update(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new EmailConfirmed(user.FirstName, user.Id);
    }
}