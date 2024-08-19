using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MRB.Communication.Requests.Users;
using Xunit;

namespace MRB.WebApiTest.Users.Login;

public class LoginUserIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    public LoginUserIntegrationTest(CustomWebApplicationFactory factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _userPassword = factory.GetUserPasword();
        httpClient = factory.CreateClient();
    }

    private readonly string method = "user/signin";
    private readonly string _name;
    private readonly string _email;
    private readonly string _password;
    private readonly string _userPassword;
    private readonly HttpClient httpClient;

    [Fact]
    public async Task SUCESSO()
    {
        var request = new RequestLoginUserJson()
        {
            Email = _email,
            Password = _userPassword
        };

        var response = await httpClient.PostAsJsonAsync(method, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should()
            .NotBeNullOrEmpty();
    }
}