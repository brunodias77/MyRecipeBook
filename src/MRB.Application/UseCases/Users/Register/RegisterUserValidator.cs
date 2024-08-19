using FluentValidation;
using MRB.Communication.Requests.Users;
using MRB.Domain.Extensions;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6)
            .WithMessage(ResourceMessagesException.PASSWORD_GREATER_THAN_OR_EQUAL_TO);
        When(user => user.Email.NotEmpty(),
            () => { RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID); });
    }
}