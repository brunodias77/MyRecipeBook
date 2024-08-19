using Bogus;
using MRB.Communication.Requests.Users;

namespace MRB.CommonTest.Requests.Users;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name, null, "outlook.com"));
    }
}