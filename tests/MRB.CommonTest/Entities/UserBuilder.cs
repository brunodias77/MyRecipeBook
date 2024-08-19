using Bogus;
using MRB.Domain.Entities;

namespace MRB.CommonTest.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        // var passwordEncripter = PasswordEncripterBuilder.Build();
        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, () => Guid.NewGuid())
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (f) => f.Internet.Password(10));

        return (user, password);
    }
}