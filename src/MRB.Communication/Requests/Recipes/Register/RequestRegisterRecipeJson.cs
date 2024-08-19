using MRB.Communication.Requests.Instructions;
using MRB.Domain.Enums;

namespace MRB.Communication.Requests.Recipes.Register;

public class RequestRegisterRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestInstructionsJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}