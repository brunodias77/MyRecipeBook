using MRB.Communication.Responses.Recipes;

namespace MRB.Application.UseCases.Dashboard.GetRecipes;

public interface IGetDashboardUseCase
{
    Task<ResponseRecipeJson> Execute();
}