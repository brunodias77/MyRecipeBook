using AutoMapper;
using MRB.Communication.Responses.Users;
using MRB.Domain.Services;

namespace MRB.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();
        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}