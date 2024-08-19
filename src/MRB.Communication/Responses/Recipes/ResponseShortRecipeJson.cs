namespace MRB.Communication.Responses.Recipes;

public class ResponseShortRecipeJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AmountIngredients { get; set; }
    
    public string? ImageUrl { get; set; }
}