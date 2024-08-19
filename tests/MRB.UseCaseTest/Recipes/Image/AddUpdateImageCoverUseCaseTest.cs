using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using MRB.UseCaseTest.Recipes.InlineData;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Image;

public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.Should().NotThrowAsync();
    }
    
    
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success_Recipe_Did_Not_Have_Image(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        recipe.ImageIdentifier = null;

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.Should().NotThrowAsync();

        recipe.ImageIdentifier.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Error_Recipe_NotFound(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(Guid.NewGuid(), file);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var file = FormFileBuilder.Txt();

        var act = async () => await useCase.Execute(recipe.Id, file);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
    }

    
    private static AddUpdateImageCoverUseCase CreateUseCase(
        User user,
        Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new AddUpdateImageCoverUseCase(loggedUser, repository, unitOfWork, blobStorage);
    }
}