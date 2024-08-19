using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.IdEncrypter;
using MRB.CommonTest.Tokens;
using MRB.Exceptions;
using MRB.WebApiTest.InlineData;
using Xunit;

namespace MRB.WebApiTest.Recipes.GetById;

public class GetRecipeByIdIntegrationTest : MyRecipesClassFixture
{
    private const string METHOD = "Recipe";
    private readonly Guid _userId;
    private readonly Guid _recipeId;
    private readonly string _recipeTitle;

    public GetRecipeByIdIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _recipeId = factory.GetRecipeId();
        _recipeTitle = factory.GetRecipeTitle();
    }

    [Fact]
    public async Task SUCESSO()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var response = await DoGet($"{METHOD}/{_recipeId}", token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("id").GetGuid().Should().Be(_recipeId);
        responseData.RootElement.GetProperty("title").GetString().Should().Be(_recipeTitle);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ERRO_RECEITA_NAO_ENCONTRADA(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var id = Guid.NewGuid();
        var response = await DoGet($"{METHOD}/{id}", token, culture);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage =
            ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));
        erros.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}