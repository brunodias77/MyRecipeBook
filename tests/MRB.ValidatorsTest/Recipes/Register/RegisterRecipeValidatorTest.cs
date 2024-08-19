using FluentAssertions;
using MRB.Application.UseCases.Recipes;
using MRB.CommonTest.Requests.Recipes;
using MRB.Domain.Enums;
using MRB.Exceptions;
using Xunit;

namespace MRB.ValidatorsTest.Recipes.Register;

public class RegisterRecipeValidatorTest
{
    [Fact]
    public void SUCESSO()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ERRO_TEMPO_DE_PREPARO_INVALIDO()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = (CookingTime?)1000;
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void ERRO_DIFICULDADE_INVALIDA()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = (Difficulty?)1000;
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORT));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData("")]
    public void ERRO_TITULO_VAZIO(string title)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title;
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }

    [Fact]
    public void SUCESSO_TEMPO_DE_PREPARO_NULO()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = null;
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SUCESSO_TIPO_DE_PRATO_VAZIO()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ERRO_TIPO_DE_PRATO_INVALIDO()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORT));
    }

    [Fact]
    public void ERRO_INGREDIENTES_VAZIOS()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_MINIMUM_ONE_INGREDIENT));
    }

    [Fact]
    public void ERRO_INSTRUCOES_VAZIAS()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
    }

    [Theory]
    [InlineData("       ")]
    [InlineData("")]
    [InlineData(null)]
    public void ERRO_INGREDIENTE_VALOR_VAZIO(string ingredient)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient);
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Fact]
    public void ERRO_MESMO_PASSO_NAS_INSTRUCOES()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
    }

    [Fact]
    public void ERRO_NUMERO_DE_PASSOS_NEGATIVO_DAS_INSTRUCOES()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = -1;
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
    }

    [Theory]
    [InlineData("       ")]
    [InlineData("")]
    [InlineData(null)]
    public void ERRO_INSTRUCAO_VALOR_VAZIO(string instruction)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = instruction;
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }
}