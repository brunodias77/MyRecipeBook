using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MRB.Domain.Dtos;
using MRB.Domain.Entities;
using MRB.Domain.Enums;
using MRB.Domain.Extensions;
using MRB.Domain.Repositories;
using DishType = MRB.Domain.Enums.DishType;

namespace MRB.Infra.Data.Repositories;

public class RecipeRepository : IRecipeRepository
{
    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<Recipe>> GetAll()
    {
        return await _context.Recipes
            .Include(r => r.Instructions)
            .Include(r => r.Ingredients)
            .Include(r => r.DishTypes)
            .ToListAsync();
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        // return await _context.Recipes.Where(r => r.UserId == user.Id).ToListAsync();        // return await _context.Recipes

        // Inicia a query com receitas ativas e do usuário especificado
        var query = _context.Recipes.AsNoTracking()
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        // Inclui os ingredientes na query
        query = query.Include(r => r.Ingredients);

        query = query.Include(r => r.Instructions);

        // // Filtra por título da receita ou ingredientes, se especificado
        if (!string.IsNullOrWhiteSpace(filters.RecipeTitle_Ingredient))
        {
            query = query.Where(r => r.Title.Contains(filters.RecipeTitle_Ingredient) ||
                                     r.Ingredients.Any(i => i.Item.Contains(filters.RecipeTitle_Ingredient)));
        }

        // Filtra por dificuldades, se especificado
        if (filters.Difficulties != null && filters.Difficulties.Any())
        {
            query = query.Where(recipe => filters.Difficulties.Contains((Difficulty)recipe.Difficulty));
        }

        // Filtra por Tempo de cozimento.
        if (filters.CookingTimes != null && filters.CookingTimes.Any())
        {
            query = query.Where(recipe => filters.CookingTimes.Contains((CookingTime)recipe.CookingTime));
        }

        //Filtra por tipo de prato.
        if (filters.DishTypes != null && filters.DishTypes.Any())
        {
            query = query.Where(recipe =>
                recipe.DishTypes.Any(dishType => filters.DishTypes.Contains((MRB.Domain.Enums.DishType)dishType.Type)));
        }

        // // Retorna a lista de receitas filtradas
        return await query.ToListAsync();
    }

    public async Task<Recipe> GetById(User user, Guid recipeId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public async Task<Recipe> GetById_AsNoTracking(User user, Guid recipeId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public async Task Delete(Guid recipeId)
    {
        var recipe = await _context.Recipes.FindAsync(recipeId);
        _context.Recipes.Remove(recipe);
    }

    public void Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
    }

    public async Task<IList<Recipe>> GetForDashboard(User user)
    {
        return await _context.Recipes.AsNoTracking().Include(c => c.Ingredients)
            .Where(r => r.Active && r.UserId == user.Id).OrderByDescending(r => r.CreatedOn).Take(5).ToListAsync();
    }

    private IIncludableQueryable<Recipe, ICollection<Domain.Entities.DishType>> GetFullRecipe()
    {
        return _context.Recipes
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes);
    }
}