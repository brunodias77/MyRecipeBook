using Microsoft.AspNetCore.Mvc;
using MRB.Api.Filters;

namespace MRB.Api.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}