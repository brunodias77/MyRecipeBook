using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Users;

namespace MRB.Application.UseCases.Users.Login;

public interface ILoginUserUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestLoginUserJson request);
}