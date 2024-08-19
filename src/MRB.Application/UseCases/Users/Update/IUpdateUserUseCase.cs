using MRB.Communication.Requests.Users;

namespace MRB.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}