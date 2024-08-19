using FluentAssertions;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests.Recipes;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Filter;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestFilterRecipeJsonBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(user, recipes);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Recipes.Should().NotBeNullOrEmpty();
        result.Recipes.Should().HaveCount(recipes.Count);
    }

    [Fact]
    public async Task ERRO_TEMPO_DE_COZIMENTO_INVALIDO()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MRB.Domain.Enums.CookingTime)1000);
        var useCase = CreateUseCase(user, recipes);
        Func<Task> act = async () => { await useCase.Execute(request); };
        (await act.Should().ThrowAsync<ErrorOnValidationException>()).Where(e =>
            e.GetErrorMessages().Count == 1 &&
            e.GetErrorMessages().Contains(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    private static FilterRecipeUseCase CreateUseCase(User user, IList<Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeRepository = new RecipeRepositoryBuilder().Filter(user, recipes).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();
        return new FilterRecipeUseCase(mapper, loggedUser, recipeRepository, blobStorage);
    }
}