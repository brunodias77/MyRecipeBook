using Bogus;
using MRB.Communication.Requests.Instructions;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Domain.Enums;

namespace MRB.CommonTest.Requests.Recipes;

public class RequestRecipeJsonBuilder
{
    public static RequestRegisterRecipeJson Build()
    {
        var faker = new Faker("pt_BR");

        // Cria um gerador de dados falso para a classe RequestInstructionsJson
        var instructionsFaker = new Faker<RequestInstructionsJson>()
            .RuleFor(i => i.Step, f => f.IndexFaker + 1)
            .RuleFor(i => i.Text, f => f.Lorem.Sentence());

        // Cria e retorna um objeto de RequestRegisterRecipeJson com dados falsos
        return new Faker<RequestRegisterRecipeJson>()
            .RuleFor(r => r.Title, f => f.Lorem.Sentence(3))
            .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(r => r.Ingredients, f => f.Make(5, () => f.Lorem.Word()))
            .RuleFor(r => r.Instructions, f => instructionsFaker.Generate(3))
            .RuleFor(r => r.DishTypes, f => f.Make(2, () => f.PickRandom<DishType>()))
            .Generate();
        
    }
}