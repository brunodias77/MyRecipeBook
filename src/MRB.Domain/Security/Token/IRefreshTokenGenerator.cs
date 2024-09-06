using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRB.Domain.Security.Token
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();

    }
}