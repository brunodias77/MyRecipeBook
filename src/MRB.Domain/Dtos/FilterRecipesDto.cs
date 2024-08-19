using MRB.Domain.Enums;

namespace MRB.Domain.Dtos;

public class FilterRecipesDto
{
    public string? RecipeTitle_Ingredient { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}