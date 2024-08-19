using Bogus;
using MRB.Domain.Entities;


namespace MRB.CommonTest.Entities;

public class RecipeBuilder
{
    public static IList<Recipe> Collection(User user, uint count = 2)
    {
        var list = new List<Recipe>();
        if (count == 0)
        {
            count = 1;
        }


        for (int i = 0; i < count; i++)
        {
            var fakeRecipe = Build(user);
            list.Add(fakeRecipe);
        }

        return list;
    }

    public static Recipe Build(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, Guid.NewGuid)
            .RuleFor(r => r.Title, (f) => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, (f) => (int)f.PickRandom<MRB.Domain.Enums.CookingTime>())
            .RuleFor(r => r.Difficulty, (f) => (int)f.PickRandom<MRB.Domain.Enums.Difficulty>())
            .RuleFor(r => r.ImageIdentifier, _ => $"{Guid.NewGuid()}.png")
            .RuleFor(r => r.Ingredients, (f) => f.Make(1, () => new Ingredient
            {
                Id = Guid.NewGuid(),
                Item = f.Commerce.ProductName()
            }))
            .RuleFor(r => r.Instructions, (f) => f.Make(1, () => new Instruction
            {
                Id = Guid.NewGuid(),
                Step = 1,
                Text = f.Lorem.Paragraph()
            }))
            .RuleFor(u => u.DishTypes, (f) => f.Make(1, () => new MRB.Domain.Entities.DishType
            {
                Id = Guid.NewGuid(),
                Type = (int)f.PickRandom<MRB.Domain.Enums.DishType>()
            }))
            .RuleFor(r => r.UserId, () => user.Id);
    }
}