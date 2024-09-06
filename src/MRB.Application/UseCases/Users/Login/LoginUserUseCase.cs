using MRB.Communication.Requests.Users;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Login;

public class LoginUserUseCase : ILoginUserUseCase
{
    public LoginUserUseCase(IUserRepository userRepository, IAccessTokenGenerator accessTokenGenerator,
        IPasswordEncripter passwordEncripter, ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IRefreshTokenGenerator refreshTokenGenerator)
    {
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncripter = passwordEncripter;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginUserJson request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);
        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (user is null || !passwordMatch)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisterUserJson()
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.Id),
                RefreshToken = await CreateAndSaveRefreshToken(user)
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(User user)
    {
        var refreshToken = new RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = user.Id
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }
}