using FluentAssertions;
using MRB.Application.UseCases.Recipes.GenerateChatGpt;
using MRB.CommonTest.Dtos;
using MRB.CommonTest.OpenAI;
using MRB.CommonTest.Requests.Recipes;
using MRB.Domain.Enums;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Generate;

public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(dto);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.CookingTime.Should().Be((CookingTime)dto.CookingTime);
        result.Difficulty.Should().Be(Difficulty.Low);
    }

    [Fact]
    public async Task ERRO_INGREDIENTE_DUPLICADO()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build(count: 4);
        request.Ingredients.Add(request.Ingredients[0]);

        var useCase = CreateUseCase(dto);

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST));
    }

    private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
    {
        var generateRecipeAI = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateRecipeAI);
    }
}