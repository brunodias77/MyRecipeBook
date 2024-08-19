using Bogus;
using MRB.Communication.Requests.Recipes.GenerateChatGpt;

namespace MRB.CommonTest.Requests.Recipes;

public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build(int count = 5)
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(user => user.Ingredients, faker => faker.Make(count, () => faker.Commerce.ProductName()));
    }
}