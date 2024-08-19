using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Responses.Recipes;

namespace MRB.Application.UseCases.Recipes.Register;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request);
}