using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MRB.Communication.Responses;
using MRB.Domain.Repositories;
using MRB.Domain.Security.Token;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Api.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserRepository _userRepository;

    public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserRepository userRepository)
    {
        _accessTokenValidator = accessTokenValidator;
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
            var exist = await _userRepository.ExistActiveUserWithIdentifier(userIdentifier);

            if (!exist)
                throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token Is Expired")
            {
                TokenExpired = true
            });
        }
        catch (MyRecipesBookExceptionBase ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result =
                new UnauthorizedObjectResult(
                    new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authentication))
            throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);

        return authentication["Bearer ".Length..].Trim();
    }
}