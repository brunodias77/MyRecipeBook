using FluentAssertions;
using MRB.Application.UseCases.Users.Delete;
using MRB.Application.UseCases.Users.Delete.Delete;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.ServiceBus;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using Xunit;

namespace MRB.UseCaseTest.Users.Delete.Delete;

public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(user.Id);

        await act.Should().NotThrowAsync();
    }

    private static DeleteUserAccountUseCase CreateUseCase()
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorageService = new BlobStorageServiceBuilder().Build();
        var repository = new UserRepositoryBuilder().Build();

        return new DeleteUserAccountUseCase(repository, unitOfWork, blobStorageService);
    }
}