using MRB.Domain.Security.Token;
using MRB.Infra.Security.Tokens.Generator;

namespace MRB.CommonTest.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() =>
        new JwtTokenGenerator(5, signingKey: "112#YC12ZVNew]PAW{CT{u17u259j>8Av)");
}