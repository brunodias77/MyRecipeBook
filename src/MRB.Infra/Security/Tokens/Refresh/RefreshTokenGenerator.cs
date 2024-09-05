using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRB.Domain.Security.Token;

namespace MRB.Infra.Security.Tokens.Refresh
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}