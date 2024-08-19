using MRB.Domain.Extensions;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.Storage;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    public DeleteRecipeUseCase(ILoggedUser loggedUser, IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public async Task Execute(Guid recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);
        if (recipe is null)
        {
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        if (recipe.ImageIdentifier.NotEmpty())
        {
            await _blobStorageService.Delete(loggedUser, recipe.ImageIdentifier);
        }
        await _recipeRepository.Delete(recipeId);
        await _unitOfWork.Commit();
    }
}