using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MRB.Infra.Security.Tokens;

public abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signgKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signgKey);
        return new SymmetricSecurityKey(bytes);
    }
}