using MRB.Domain.Enums;

namespace MRB.Domain.Services.OpenAI;

public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}