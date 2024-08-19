using FluentAssertions;
using MRB.Application.UseCases.Recipes.Update;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.UnitOfWork;
using MRB.Communication.Requests.Recipes.Update;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Update;

public class UpdateRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestUpdateRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user, recipe);
        Func<Task> act = async () => await useCase.Execute(recipe.Id, request);
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ERRO_RECEITA_NAO_ENCONTRADA()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(recipeId: Guid.NewGuid(), request);

        (await act.Should().ThrowAsync<NotFoundException>()).Where(e =>
            e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
    }

    [Fact]
    public async Task ERRO_TITULO_VAZIO()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestUpdateRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(recipeId: recipe.Id, request);
        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrorMessages().Should().ContainSingle()
            .Which.Should().Be(ResourceMessagesException.RECIPE_TITLE_EMPTY);
    }

    private static UpdateRecipeUseCase CreateUseCase(User user, Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var recipeRepository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();
        return new UpdateRecipeUseCase(loggedUser, recipeRepository, unitOfWork, mapper);
    }
}