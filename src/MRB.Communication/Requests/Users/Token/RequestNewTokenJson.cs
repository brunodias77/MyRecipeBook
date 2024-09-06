using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRB.Communication.Requests.Users.Token
{
    public class RequestNewTokenJson
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}