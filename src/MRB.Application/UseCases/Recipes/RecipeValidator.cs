using FluentValidation;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Exceptions;

namespace MRB.Application.UseCases.Recipes;

public class RecipeValidator : AbstractValidator<RequestRegisterRecipeJson>
{
    public RecipeValidator() // Construtor da classe RecipeValidator.
    {
        // Define uma regra para o campo Title, que não pode estar vazio. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessagesException.RECIPE_TITLE_EMPTY);

        // Define uma regra para o campo CookingTime, que deve ser um valor válido de enumeração. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.CookingTime).IsInEnum()
            .WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);

        // Define uma regra para o campo Difficulty, que deve ser um valor válido de enumeração. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.Difficulty).IsInEnum()
            .WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORT);

        // Define uma regra para a contagem dos ingredientes, que deve ser maior que 0. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0)
            .WithMessage(ResourceMessagesException.RECIPE_MINIMUM_ONE_INGREDIENT);

        // Define uma regra para a contagem das instruções, que deve ser maior que 0. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0)
            .WithMessage(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);

        // Define uma regra para cada tipo de prato na lista DishTypes, que deve ser um valor válido de enumeração. Caso contrário, lança uma mensagem de erro específica.
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORT);

        // Define uma regra para cada ingrediente na lista Ingredients, que não pode estar vazio. Caso contrário, lança uma mensagem de erro específica.
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);

        // Define regras para cada instrução na lista Instructions.
        RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
        {
            // Para cada instrução, verifica se o campo Step é maior que 0. Caso contrário, lança uma mensagem de erro específica.
            instructionRule.RuleFor(instruction => instruction.Step).GreaterThan(-1)
                .WithMessage(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP);

            // Para cada instrução, verifica se o campo Text não está vazio e tem no máximo 2000 caracteres. Caso contrário, lança uma mensagem de erro específica.
            instructionRule.RuleFor(instruction => instruction.Text).NotEmpty()
                .WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY).MaximumLength(2000)
                .WithMessage(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        });

        // Define uma regra para a lista de instruções, verificando se não há dois ou mais passos (Step) com o mesmo valor. Caso contrário, lança uma mensagem de erro específica.
        RuleFor(recipe => recipe.Instructions).Must(instructions =>
                instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }
}