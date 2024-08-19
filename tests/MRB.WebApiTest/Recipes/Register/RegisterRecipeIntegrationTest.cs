namespace MRB.WebApiTest.Recipes.Register;

using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.Tokens;
using MRB.Exceptions.Exceptions;
using MRB.WebApiTest;
using Xunit;

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
        var request = RequestRegisterRecipeFormDataBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPostFormData(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
    }
}