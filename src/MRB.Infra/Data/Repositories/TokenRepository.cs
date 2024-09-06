using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> Get(string refreshToken)
        {
            return await _context
                        .RefreshTokens
                        .AsNoTracking()
                        .Include(token => token.User)
                        .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = _context.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

            _context.RefreshTokens.RemoveRange(tokens);

            await _context.RefreshTokens.AddAsync(refreshToken);
        }
    }
}