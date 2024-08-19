using MRB.Communication.Requests.Recipes.GenerateChatGpt;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;

namespace MRB.Application.UseCases.Recipes.GenerateChatGpt;

public interface IGenerateRecipeUseCase
{
    Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
}