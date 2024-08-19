using FluentValidation;
using MRB.Communication.Requests.Users;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Users.Login;

public class LoginUserValidator : AbstractValidator<RequestLoginUserJson>
{
    public LoginUserValidator()
    {
        RuleFor(user => user).Must(user =>
                !string.IsNullOrWhiteSpace(user.Email) || !string.IsNullOrWhiteSpace(user.Password))
            .WithMessage(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6)
            .WithMessage(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
    }
}