using MRB.Application.UseCases.Users.Register;
using MRB.CommonTest.Requests.Users;
using Xunit;
using FluentAssertions;
using MRB.CommonTest.Cryptography;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Tokens;
using MRB.CommonTest.UnitOfWork;

namespace MRB.UseCaseTest.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async void SUCESSO()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRepository = new UserRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (!string.IsNullOrEmpty(email))
        {
            userRepository.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(userRepository.Build(), unitOfWork, passwordEncripter,
            accessTokenGenerator, mapper);
    }
}