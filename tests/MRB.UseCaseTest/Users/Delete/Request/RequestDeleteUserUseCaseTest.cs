using Xunit;

namespace MRB.UseCaseTest.Users.Delete.Request;

using FluentAssertions;
using MRB.Application.UseCases.Users.Delete;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.ServiceBus;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;

public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();

        user.Active.Should().BeFalse();
    }

    private static RequestDeleteUserUseCase CreateUseCase(User user)
    {
        var queue = DeleteUserQueueBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserRepositoryBuilder().GetById(user).Build();

        return new RequestDeleteUserUseCase(queue, repository, loggedUser, unitOfWork);
    }
}