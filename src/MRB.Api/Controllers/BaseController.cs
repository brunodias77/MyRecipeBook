using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MRB.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected static bool IsNotAutheticated(AuthenticateResult authenticate)
        {
            return !authenticate.Succeeded || authenticate.Principal is null || !authenticate.Principal.Identities.Any(id => id.IsAuthenticated);
        }
    }
}