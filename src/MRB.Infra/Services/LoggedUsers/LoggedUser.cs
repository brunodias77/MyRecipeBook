using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MRB.Domain.Entities;
using MRB.Domain.Security.Token;
using MRB.Domain.Services;
using MRB.Infra.Data;

namespace MRB.Infra.Services.LoggedUsers;

public class LoggedUser(ApplicationDbContext dbContext, ITokenProvider token) : ILoggedUser
{
    public async Task<User> User()
    {
        // Obtém o valor do token atual.
        var token1 = token.Value();

        // Cria um manipulador para o token JWT.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Lê e valida o token JWT.
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token1);

        // Obtém o valor do claim de identificação (SID) do token JWT.
        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        // Converte o identificador do usuário para um Guid.
        var userIdentifier = Guid.Parse(identifier);

        // Busca o usuário no banco de dados que está ativo e corresponde ao identificador obtido do token.
        return await dbContext.Users
            .AsNoTracking() // Utiliza AsNoTracking para melhorar a performance, já que não precisa rastrear as mudanças nas entidades.
            .FirstAsync(user =>
                user.Active &&
                user.Id == userIdentifier); // Encontra o primeiro usuário ativo com o identificador correspondente.
    }
}