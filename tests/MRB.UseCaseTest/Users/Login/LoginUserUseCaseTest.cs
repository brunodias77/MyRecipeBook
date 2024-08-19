using FluentAssertions;
using MRB.Application.UseCases.Users.Login;
using MRB.CommonTest.Cryptography;
using MRB.CommonTest.Entities;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Tokens;
using MRB.Communication.Requests.Users;
using MRB.Domain.Entities;
using Xunit;

namespace MRB.UseCaseTest.Users.Login;

public class LoginUserUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestLoginUserJson
        {
            Email = user.Email,
            Password = password,
        };

        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    private static LoginUserUseCase CreateUseCase(User? user, string? password = null)
    {
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
        var userRepositoryBuilder = new UserRepositoryBuilder().GetUserByEmail(user).Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new LoginUserUseCase(userRepositoryBuilder, accessTokenGenerator, passwordEncripter);
    }
}