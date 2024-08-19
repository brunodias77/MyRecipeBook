namespace MRB.Application.UseCases.Recipes.Delete;

public interface IDeleteRecipeUseCase
{
    Task Execute(Guid recipeId);
}