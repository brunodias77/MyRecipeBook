using System.Net;
using FluentAssertions;
using MRB.CommonTest.Requests.Users;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Users.Update;

public class UpdateUserInvalidTokenTest : MyRecipesClassFixture
{
    private readonly string METHOD = "user";

    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(METHOD, request, token: "tokenInvalid");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(METHOD, request, token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoPut(METHOD, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}