using System.Net;
using FluentAssertions;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Recipes.Generate;

public class GenerateRecipeInvalidTokenTest : MyRecipesClassFixture
{
    private const string METHOD = "recipe/generate";

    public GenerateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}