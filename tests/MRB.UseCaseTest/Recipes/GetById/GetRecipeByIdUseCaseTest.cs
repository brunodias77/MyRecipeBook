using FluentAssertions;
using MRB.Application.UseCases.Recipes.GetById;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using MRB.Infra.Services.LoggedUsers;
using Xunit;

namespace MRB.UseCaseTest.Recipes.GetById;

public class GetRecipeByIdUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user, recipe);
        var result = await useCase.Execute(recipe.Id);
        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(recipe.Title);
    }

    [Fact]
    public async Task ERRO_RECEITA_NAO_ENCONTRADA()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => { await useCase.Execute(Guid.NewGuid()); };
        (await act.Should().ThrowAsync<NotFoundException>()).Where(e =>
            e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
    }

    private static GetRecipeByIdUseCase CreateUseCase(User user, Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().GetById_AsNoTracking(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new GetRecipeByIdUseCase(mapper, loggedUser, repository, blobStorage);
    }
}