using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Requests.Recipes.Update;

namespace MRB.Application.UseCases.Recipes.Update;

public interface IUpdateRecipeUseCase
{
    Task Execute(Guid id, RequestUpdateRecipeJson request);
}