using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRB.Domain.Entities;

namespace MRB.Domain.Repositories
{
    public interface ITokenRepository
    {
        Task<RefreshToken> Get(string refreshToken);
        Task SaveNewRefreshToken(RefreshToken refreshToken);

    }
}