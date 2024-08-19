using Microsoft.AspNetCore.Http;

namespace MRB.Application.UseCases.Recipes.Image;

public interface IAddUpdateImageCoverUseCase
{
    Task Execute(Guid recipeId, IFormFile file);
}