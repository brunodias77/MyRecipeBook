using MRB.Communication.Requests.Recipes.GenerateChatGpt;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;
using MRB.Domain.Enums;
using MRB.Domain.Services.OpenAI;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.GenerateChatGpt;

public class GenerateRecipeUseCase : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAI _generator;

    public GenerateRecipeUseCase(IGenerateRecipeAI generator)
    {
        _generator = generator;
    }


    public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        Validate(request);
        var response = await _generator.Generate(request.Ingredients);
        return new ResponseGeneratedRecipeJson
        {
            Title = response.Title,
            CookingTime = (CookingTime)response.CookingTime,
            Instructions = response.Instructions.Select(c => new ResponseGeneratesInstructionJson
            {
                Step = c.Step,
                Text = c.Text
            }).ToList(),
            Difficulty = MRB.Domain.Enums.Difficulty.Low
        };
    }

    private static void Validate(RequestGenerateRecipeJson request)
    {
        var result = new GenerateRecipeValidator().Validate(request);
        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}