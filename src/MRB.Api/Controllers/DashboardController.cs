using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MRB.Api.Attributes;
using MRB.Application.UseCases.Dashboard.GetRecipes;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;

namespace MRB.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AuthenticatedUser]
public class DashboardController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Recipes.Any())
        {
            return Ok(response);
        }

        return NoContent();
    }
}