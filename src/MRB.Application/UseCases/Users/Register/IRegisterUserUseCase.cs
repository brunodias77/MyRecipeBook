using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Users;

namespace MRB.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}