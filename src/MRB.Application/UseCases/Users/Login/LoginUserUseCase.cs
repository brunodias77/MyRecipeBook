using MRB.Communication.Requests.Users;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Users;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Login;

public class LoginUserUseCase : ILoginUserUseCase
{
    public LoginUserUseCase(IUserRepository userRepository, IAccessTokenGenerator accessTokenGenerator,
        IPasswordEncripter passwordEncripter)
    {
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncripter = passwordEncripter;
    }

    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordEncripter _passwordEncripter;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginUserJson request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        return new ResponseRegisterUserJson()
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.Id)
            }
        };
    }
}