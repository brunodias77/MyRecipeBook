using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MRB.Exceptions.Exceptions
{
    public class RefreshTokenExpiredException : MyRecipesBookExceptionBase
    {
        public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
    }
}