using MRB.Domain.Enums;

namespace MRB.Communication.Responses.Recipes;

public class ResponseCompleteRecipeJson
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IList<ResponseIngredientJson> Ingredients { get; set; } = [];
    public IList<ResponseInstructionJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public string? ImageUrl { get; set; }
}