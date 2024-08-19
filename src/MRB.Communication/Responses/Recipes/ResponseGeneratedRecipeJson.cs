using MRB.Domain.Entities;
using MRB.Domain.Enums;

namespace MRB.Communication.Responses.Recipes;

public class ResponseGeneratedRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public IList<Ingredient> Ingredients { get; set; } = [];
    public MRB.Domain.Enums.CookingTime CookingTime { get; set; }
    public IList<ResponseGeneratesInstructionJson> Instructions { get; set; } = [];
    public Difficulty Difficulty { get; set; }
}