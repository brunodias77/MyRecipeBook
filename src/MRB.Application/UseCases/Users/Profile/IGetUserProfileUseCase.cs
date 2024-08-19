using MRB.Communication.Responses.Users;

namespace MRB.Application.UseCases.Users.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}