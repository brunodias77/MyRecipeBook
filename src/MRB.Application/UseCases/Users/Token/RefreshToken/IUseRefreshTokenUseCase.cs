using MRB.Communication.Requests.Users.Token;
using MRB.Communication.Responses;

namespace MRB.Application.UseCases.Users.Token.RefreshToken
{
    public interface IUseRefreshTokenUseCase
    {
        Task<ResponseTokenJson> Execute(RequestNewTokenJson request);

    }
}