using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.Tokens;
using MRB.Exceptions.Exceptions;
using MRB.WebApiTest;
using Xunit;

namespace MRB.WebApiTest.Recipes;

public class RegisterRecipeIntegrationTest : MyRecipesClassFixture
{
    private readonly string METHOD = "recipe";
    private readonly Guid _userIdentifier;

    public RegisterRecipeIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //Pegando o conteudo da requisição
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        //Recuperando as informações do json
        responseData.RootElement.GetProperty("title").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Title);
    }
}