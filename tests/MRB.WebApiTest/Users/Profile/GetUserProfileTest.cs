using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Users.Profile;

public class GetUserProfileTest : MyRecipesClassFixture
{
    private readonly string METHOD = "User/profile";

    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(_name);

        responseData.RootElement.GetProperty("email").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(_email);
    }
}