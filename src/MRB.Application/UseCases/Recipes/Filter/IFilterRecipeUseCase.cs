using MRB.Communication.Requests.Recipes.Filter;
using MRB.Communication.Responses.Recipes;

namespace MRB.Application.UseCases.Recipes.Filter;

public interface IFilterRecipeUseCase
{
    Task<ResponseRecipeJson> Execute(RequestFilterRecipeJson request);
}