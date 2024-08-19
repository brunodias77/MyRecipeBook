using AutoMapper;
using MRB.Application.Extensions;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.Storage;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    public RegisterRecipeUseCase(IRecipeRepository recipeRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork,
        IMapper mapper, IBlobStorageService blobStorageService)
    {
        _recipeRepository = recipeRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobStorageService = blobStorageService;
    }

    private readonly IRecipeRepository _recipeRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobStorageService _blobStorageService;

    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();
        var recipe = _mapper.Map<Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count; index++)
        {
            instructions[index].Step = index + 1;
        }

        recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);
        if (request.Image is not null)
        {
            var fileStream = request.Image.OpenReadStream();
            (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();
            if (!isValidImage)
            {
                throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
            }

            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";
            await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
        }
        await _recipeRepository.Add(recipe);
        return new ResponseRegisteredRecipeJson
        {
            Id = recipe.Id,
            Title = recipe.Title
        };
    }

    private static void Validate(RequestRegisterRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}