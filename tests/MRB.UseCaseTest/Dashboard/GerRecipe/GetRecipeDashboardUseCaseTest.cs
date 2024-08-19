using FluentAssertions;
using MRB.Application.UseCases.Dashboard.GetRecipes;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.Domain.Entities;
using Xunit;

namespace MRB.UseCaseTest.Dashboard.GerRecipe;

public class GetRecipeDashboardUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(user, recipes);
        var result = await useCase.Execute();
        result.Should().NotBeNull();
        result.Recipes
            .Should()
            .HaveCountGreaterThan(0)
            .And.OnlyHaveUniqueItems(r => r.Id)
            .And.AllSatisfy(recipe =>
            {
                // recipe.Id.Should().NotBeNullOrWhiteSpace();
                recipe.Title.Should().NotBeNullOrWhiteSpace();
                recipe.ImageUrl.Should().NotBeNullOrWhiteSpace();
                // recipe.AmountIngredients.Should().BeGreaterThan(0);
            });
    }

    private static GetDashboardUseCase CreateUseCase(User user, IList<Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().GetForDashboard(user, recipes).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();

        return new GetDashboardUseCase(repository, mapper, loggedUser, blobStorage);
    }
}