using System.Resources;
using FluentValidation;
using MRB.Communication.Requests.Recipes.GenerateChatGpt;
using MRB.Domain.ValueObjects;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Recipes.GenerateChatGpt;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maximum_number_ingredients = MyRecipesBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;
        RuleFor(req => req.Ingredients.Count).InclusiveBetween(1, maximum_number_ingredients)
            .WithMessage(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);
        RuleFor(req => req.Ingredients).Must(ing => ing.Count == ing.Distinct().Count())
            .WithMessage(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
        RuleFor(req => req.Ingredients).ForEach(rule =>
        {
            rule.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_EMPTY);
                    return;
                }

                if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                {
                    context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
                    return;
                }
            });
        });
    }
}