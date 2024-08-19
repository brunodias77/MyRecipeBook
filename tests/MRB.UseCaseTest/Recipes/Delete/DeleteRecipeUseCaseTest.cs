using FluentAssertions;
using MRB.Application.UseCases.Recipes.Delete;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Delete;

public class DeleteRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user, recipe);
        Func<Task> act = async () => { await useCase.Execute(recipe.Id); };
        await act.Should().NotThrowAsync();
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

    private static DeleteRecipeUseCase CreateUseCase(User user, Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeRepository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();
        return new DeleteRecipeUseCase(loggedUser, recipeRepository, unitOfWork, blobStorage);
    }
}