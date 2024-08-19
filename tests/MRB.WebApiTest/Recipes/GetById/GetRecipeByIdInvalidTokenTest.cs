using System.Net;
using FluentAssertions;
using MRB.CommonTest.IdEncrypter;
using MRB.CommonTest.Tokens;
using Xunit;

namespace MRB.WebApiTest.Recipes.GetById;

public class GetRecipeByIdInvalidTokenTest : MyRecipesClassFixture
{
    private const string METHOD = "Recipe";

    public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ERRO_TOKEN_INVALIDO()
    {
        var id = IdEncrypterBuilder.Build();
        var response = await DoGet($"{METHOD}/{id}", token: "token invalid");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ERRO_SEM_TOKEN()
    {
        var id = IdEncrypterBuilder.Build();
        var response = await DoGet($"{METHOD}/{id}", token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TOKEN_COM_ERRO_USUARIO_NAO_ENCONTRADO()
    {
        var id = IdEncrypterBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoGet($"{METHOD}/{id}", token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}