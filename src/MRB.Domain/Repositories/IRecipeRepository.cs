using MRB.Domain.Dtos;
using MRB.Domain.Entities;

namespace MRB.Domain.Repositories;

public interface IRecipeRepository
{
    Task Add(Recipe recipe);
    Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters);
    Task<IList<Recipe>> GetAll();
    Task<Recipe> GetById(User user, Guid recipeId);
    Task<Recipe> GetById_AsNoTracking(User user, Guid recipeId);
    Task Delete(Guid recipeId);
    void Update(Recipe recipe);

    Task<IList<Recipe>> GetForDashboard(User user);
}