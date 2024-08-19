using Azure.Core;
using FluentAssertions;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.CommonTest.Requests.Recipes;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Domain.Enums;
using MRB.Exceptions;
using Xunit;

namespace MRB.ValidatorsTest.Recipes.Filter;

public class FilterRecipeValidationTest
{
    [Fact]
    public void SUCESSO()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ERRO_TEMPO_DE_COZIMENTO_INVALIDO()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void ERRO_DIFICULDADE_INVALIDA()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties.Add((Difficulty)1000);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORT));
    }

    [Fact]
    public void ERRO_TIPO_DE_PRATO_INVALIDO()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORT));
    }
}