using AutoMapper;
using MRB.Application.Extensions;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.Storage;

namespace MRB.Application.UseCases.Dashboard.GetRecipes;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IBlobStorageService _blobStorageService;

    public GetDashboardUseCase(IRecipeRepository recipeRepository, IMapper mapper, ILoggedUser loggedUser, IBlobStorageService blobStorageService)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
        _blobStorageService = blobStorageService;
    }


    public async Task<ResponseRecipeJson> Execute()
    {
        var loggedUser = await _loggedUser.User();
        var recipes = await _recipeRepository.GetForDashboard(loggedUser);
        return new ResponseRecipeJson
        {
            Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
        };
    }
}