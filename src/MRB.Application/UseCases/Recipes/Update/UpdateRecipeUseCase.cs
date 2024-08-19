using AutoMapper;
using MRB.Communication.Requests.Recipes.Update;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    public UpdateRecipeUseCase(ILoggedUser loggedUser, IRecipeRepository recipeRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task Execute(Guid recipeId, RequestUpdateRecipeJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);
        if (recipe is null)
        {
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();
        _mapper.Map(request, recipe);
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count; index++)
        {
            instructions.ElementAt(index).Step = index + 1;
        }

        recipe.Instructions = _mapper.Map<IList<MRB.Domain.Entities.Instruction>>(instructions);
        _recipeRepository.Update(recipe);
        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateRecipeJson request)
    {
        var result = new UpdateRecipeValidator().Validate(request);
        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}