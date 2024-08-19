using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Users;
using MRB.CommonTest.Tokens;
using MRB.Exceptions;
using MRB.WebApiTest.InlineData;
using Xunit;

namespace MRB.WebApiTest.Users.Update;

public class UpdateUserTest : MyRecipesClassFixture
{
    private const string METHOD = "user";
    private readonly Guid _userIdentifier;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserId();
    }

    [Fact]
    public async Task SUCESSO()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut(METHOD, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ERRO_NOME_VAZIO(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut(METHOD, request, token, culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage =
            ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}