using FluentValidation;
using MRB.Communication.Requests.Users;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        When(request => !string.IsNullOrWhiteSpace(request.Email), () =>
        {
            RuleFor(request => request.Email)
                .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}