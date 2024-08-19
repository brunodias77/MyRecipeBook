using FluentAssertions;
using MRB.Application.UseCases.Users.Update;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests.Users;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task ERRO_NOME_VAZIO()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.NAME_EMPTY));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task ERRO_EMAIL_VAZIO()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_EMPTY));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task ERRO_EMAIL_JA_REGISTRADO()
    {
        (var user, _) = UserBuilder.Build(); // Usuário antigo
        var request = RequestUpdateUserJsonBuilder.Build(); // Requisição com email já registrado
        var useCase = CreateUseCase(user, request.Email);
        Func<Task> act = async () => { await useCase.Execute(request); };
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userRepositoryBuilder = new UserRepositoryBuilder().GetById(user);

        if (!string.IsNullOrEmpty(email))
        {
            userRepositoryBuilder.ExistActiveUserWithEmail(email);
        }

        var userRepository = userRepositoryBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new UpdateUserUseCase(loggedUser, userRepository, unitOfWork);
    }
}