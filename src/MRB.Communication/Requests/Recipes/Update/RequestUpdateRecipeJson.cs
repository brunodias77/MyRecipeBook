using MRB.Communication.Requests.Instructions;
using MRB.Domain.Enums;

namespace MRB.Communication.Requests.Recipes.Update;

public class RequestUpdateRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestInstructionsJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}