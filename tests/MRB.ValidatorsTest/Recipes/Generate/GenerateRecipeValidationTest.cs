using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MRB.Application.UseCases.Recipes.GenerateChatGpt;
using MRB.CommonTest.Requests.Recipes;
using MRB.Communication.Requests.Recipes.GenerateChatGpt;
using MRB.Domain.ValueObjects;
using MRB.Exceptions;
using Xunit;

namespace MRB.ValidatorsTest.Recipes.Generate;

public class GenerateRecipeValidationTest
{
    [Fact]
    public void SUCESSO()
    {
        {
            var validator = new GenerateRecipeValidator();
            var request = RequestGenerateRecipeJsonBuilder.Build();
            var result = validator.Validate(request);
            result.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void ERRO_MAIS_INGREDIENTES_QUE_O_PERMITIDO()
    {
        var validator = new GenerateRecipeValidator();
        var request =
            RequestGenerateRecipeJsonBuilder.Build(
                count: MyRecipesBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS));
    }

    [Fact]
    public void ERRO_INGREDIENTE_DUPLICADO()
    {
        var validator = new GenerateRecipeValidator();
        var request =
            RequestGenerateRecipeJsonBuilder.Build(
                count: MyRecipesBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients[0]);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters",
        Justification = "Because it is a unit test")]
    public void ERRO_INGREDIENTE_VAZIO(string ingredient)
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGenerateRecipeJsonBuilder.Build(count: 1);
        request.Ingredients.Add(ingredient);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Fact]
    public void ERRO_INGREDIENTE_QUE_NAO_SEGUE_O_PADRAO()
    {
        var validator = new GenerateRecipeValidator();

        var request =
            RequestGenerateRecipeJsonBuilder.Build(
                count: MyRecipesBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add("This is an invalid ingredient because is too long");

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN));
    }
}