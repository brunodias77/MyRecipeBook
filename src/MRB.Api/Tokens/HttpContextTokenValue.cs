using MRB.Domain.Security.Token;

namespace MRB.Api.Tokens;

public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Value()
    {
        var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return authentication["Bearer ".Length..].ToString();
    }
}