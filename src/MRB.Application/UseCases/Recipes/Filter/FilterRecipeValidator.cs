using FluentValidation;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Recipes.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
{
    public FilterRecipeValidator()
    {
        RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        RuleForEach(r => r.Difficulties).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORT);
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORT);
    }
}