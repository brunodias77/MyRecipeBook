using MRB.Communication.Requests.Users;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    public UpdateUserUseCase(ILoggedUser loggedUser, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    private readonly ILoggedUser _loggedUser;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);

        var user = await _userRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _userRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);


        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _userRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email",
                    ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessage = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}