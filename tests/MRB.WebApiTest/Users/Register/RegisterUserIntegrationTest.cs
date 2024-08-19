using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Users;
using MRB.Exceptions;
using MRB.WebApiTest.InlineData;
using Xunit;

namespace MRB.WebApiTest.Users.Register;

public class RegisterUserIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    public RegisterUserIntegrationTest(CustomWebApplicationFactory factory)
    {
        this.httpClient = factory.CreateClient();
    }

    private readonly HttpClient httpClient;
    private readonly string method = "user/signup";

    [Fact]
    public async Task SUCESSO()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await httpClient.PostAsJsonAsync(method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ERRO_NOME_VAZIO(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        if (httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
        {
            httpClient.DefaultRequestHeaders.Remove("Accept-Language");
        }

        httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await httpClient.PostAsJsonAsync(method, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage =
            ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should()
            .ContainSingle()
            .And
            .Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}