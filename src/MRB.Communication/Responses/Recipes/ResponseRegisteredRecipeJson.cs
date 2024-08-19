namespace MRB.Communication.Responses.Recipes;

public class ResponseRegisteredRecipeJson
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
}