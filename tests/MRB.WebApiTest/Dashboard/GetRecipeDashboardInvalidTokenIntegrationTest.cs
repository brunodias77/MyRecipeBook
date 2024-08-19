using System.Net;
using FluentAssertions;
using MRB.CommonTest.IdEncrypter;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Dashboard;

public class GetRecipeDashboardInvalidTokenIntegrationTest : MyRecipesClassFixture
{
    private const string METHOD = "Dashboard";

    public GetRecipeDashboardInvalidTokenIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ERRO_TOKEN_INVALIDO()
    {
        var response = await DoGet(METHOD, token: "token invalid");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ERRO_SEM_TOKEN()
    {
        var response = await DoGet(METHOD, token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TOKEN_COM_ERRO_USUARIO_NAO_ENCONTRADO()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoGet(METHOD, token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}