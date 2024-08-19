using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MRB.Application.UseCases.Recipes;
using MRB.Application.UseCases.Recipes.Register;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using MRB.UseCaseTest.Recipes.InlineData;
using Xunit;


namespace MRB.UseCaseTest.Recipes.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        // result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task SUCESSO_COM_IMAGEM(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build(file);

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task ERRO_TIPO_DE_ARQUIVO_INVALIDO()
    {
        (var user, _) = UserBuilder.Build();

        var textFile = FormFileBuilder.Txt();

        var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
    }

    // [Fact]
    // public async Task ERRO_TITULO_VAZIO()
    // {
    //     // Arrange
    //     var (user, _) = UserBuilder.Build();
    //     var request = RequestRegisterRecipeFormDataBuilder.Build();
    //     request.Title = string.Empty; // Título vazio para disparar a validação
    //     var useCase = CreateUseCase(user);

    //     // Act
    //     Func<Task> act = async () => { await useCase.Execute(request); };

    //     // Assert
    //     (await act.Should().ThrowAsync<ErrorOnValidationException>())
    //         .Where(e => e.GetErrorMessages().Count == 1 &&
    //             e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
    // }

    [Fact]
    public async Task ERRO_INSTRUCOES_MUITO_LONGAS()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(2001);
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
    }

    private static RegisterRecipeUseCase CreateUseCase(User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeRepositoryBuilder = new RecipeRepositoryBuilder();
        var repository = recipeRepositoryBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unitOfWork, mapper, blobStorage);
    }
}