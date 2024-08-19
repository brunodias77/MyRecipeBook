using MRB.Communication.Requests.Recipes.Filter;
using MRB.Communication.Responses.Recipes;

namespace MRB.Application.UseCases.Recipes.GetById;

public interface IGetRecipeByIdUseCase
{
    Task<ResponseCompleteRecipeJson> Execute(Guid recipeId);
}