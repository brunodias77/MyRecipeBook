using MRB.Domain.Enums;

namespace MRB.Communication.Requests.Recipes.Filter;

public class RequestFilterRecipeJson
{
    public string? RecipeTitle_Ingredient { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}