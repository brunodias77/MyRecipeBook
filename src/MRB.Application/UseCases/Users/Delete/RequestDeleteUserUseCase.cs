using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.ServiceBus;

namespace MRB.Application.UseCases.Users.Delete;

public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
    public RequestDeleteUserUseCase(IDeleteUserQueue queue, IUserRepository repository, ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _queue = queue;
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    private readonly IDeleteUserQueue _queue;
    private readonly IUserRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();
        var user = await _repository.GetById(loggedUser.Id);
        user.Active = false;
        _repository.Update(user);
        await _unitOfWork.Commit();
        await _queue.SendMessage(loggedUser);
    }
}