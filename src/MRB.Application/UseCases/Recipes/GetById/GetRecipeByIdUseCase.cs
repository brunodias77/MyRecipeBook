using AutoMapper;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Extensions;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.Storage;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeRepository recipeRepository,
        IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _blobStorageService = blobStorageService;
    }

    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IBlobStorageService _blobStorageService;


    public async Task<ResponseCompleteRecipeJson> Execute(Guid recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById_AsNoTracking(loggedUser, recipeId);
        if (recipe is null) throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
        var response = _mapper.Map<ResponseCompleteRecipeJson>(recipe);
        if (recipe.ImageIdentifier.NotEmpty())
        {
            var url = await _blobStorageService.GetFileUrl(loggedUser, recipe.ImageIdentifier);

            response.ImageUrl = url;
        }

        return response;
    }
}