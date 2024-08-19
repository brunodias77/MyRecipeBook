using MRB.Domain.Enums;

namespace MRB.Communication.Responses.Recipes;

public class ResponseGenerateRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<ResponseGeneratesInstructionJson> Instructions { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
}