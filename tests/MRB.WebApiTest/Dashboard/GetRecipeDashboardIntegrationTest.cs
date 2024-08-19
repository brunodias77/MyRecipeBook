using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Dashboard;

public class GetRecipeDashboardIntegrationTest : MyRecipesClassFixture
{
    private const string METHOD = "Dashboard";
    private readonly Guid _userId;

    public GetRecipeDashboardIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task SUCESSO()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var response = await DoGet(METHOD, token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
    }
}