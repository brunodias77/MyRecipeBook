using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MRB.Api.Attributes;
using MRB.Application.UseCases.Users.Delete;
using MRB.Application.UseCases.Users.Login;
using MRB.Application.UseCases.Users.Login.External;
using MRB.Application.UseCases.Users.Profile;
using MRB.Application.UseCases.Users.Register;
using MRB.Application.UseCases.Users.Token.RefreshToken;
using MRB.Application.UseCases.Users.Update;
using MRB.Communication.Requests.Users;
using MRB.Communication.Requests.Users.Token;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Security;

namespace MRB.Api.Controllers;

public class UserController : BaseController
{
    public UserController(IUserRepository userRepository, IPasswordEncripter passwordEncripter)
    {
        _userRepository = userRepository;
        _passwordEncripter = passwordEncripter;
    }

    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncripter _passwordEncripter;

    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        var users = await _userRepository.GetAll();
        return Ok(users);
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginUserUseCase useCase,
        [FromBody] RequestLoginUserJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> Delete([FromServices] IRequestDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }

    [HttpGet]
    [Route("google")]
    public async Task<IActionResult> LoginGoogle(String returnUrl, [FromServices] IExternalLoginUseCase useCase)
    {

        var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (IsNotAutheticated(authenticate))
        {
            return Challenge(GoogleDefaults.AuthenticationScheme);
        }
        else
        {
            var claims = authenticate.Principal!.Identities.First().Claims;
            var name = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name).Value;
            var email = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var token = await useCase.Execute(name, email);
            return Redirect($"{returnUrl}/{token}");
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}