namespace MRB.Domain.Enums;

public class GeneratedRecipeDto
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public IList<GeneratedInstructionDto> Instructions { get; set; } = [];
}