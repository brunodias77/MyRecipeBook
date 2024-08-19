using Bogus;
using Microsoft.AspNetCore.Http;
using MRB.Communication.Requests.Instructions;
using MRB.Communication.Requests.Recipes;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Domain.Enums;

namespace MRB.CommonTest.Requests.Recipes
{
    public class RequestRegisterRecipeFormDataBuilder
    {
        public static RequestRegisterRecipeFormData Build(IFormFile? formFile = null)
        {

            var instructionsFaker = new Faker<RequestInstructionsJson>()
                .RuleFor(i => i.Step, f => f.IndexFaker + 1)
                .RuleFor(i => i.Text, f => f.Lorem.Sentence());

            var step = 1;

            return new Faker<RequestRegisterRecipeFormData>()
                .RuleFor(r => r.Image, _ => formFile)
                .RuleFor(r => r.Title, f => f.Lorem.Word())
                .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
                .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
                .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
                .RuleFor(r => r.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()))
                .RuleFor(r => r.Instructions, f => instructionsFaker.Generate(3));
        }
    }
}