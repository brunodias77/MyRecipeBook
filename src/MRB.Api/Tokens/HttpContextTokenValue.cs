using MRB.Domain.Security.Token;

namespace MRB.Api.Tokens
{
    // Implementação de ITokenProvider que obtém o token do contexto HTTP
    public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        // Acesso ao contexto HTTP para ler os headers da solicitação
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        // Obtém o valor do token a partir do header Authorization da solicitação HTTP
        public string Value()
        {
            // Obtém o valor do header Authorization
            var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            // Remove o prefixo "Bearer " e retorna o token
            return authentication["Bearer ".Length..].ToString();
        }
    }
}
