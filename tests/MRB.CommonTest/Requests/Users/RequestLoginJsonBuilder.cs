using Bogus;
using MRB.Communication.Requests.Users;

namespace MRB.CommonTest.Requests.Users;

public class RequestLoginJsonBuilder
{
    public static RequestLoginUserJson Build(int passwordLengh = 10)
    {
        return new Faker<RequestLoginUserJson>()
            .RuleFor(user => user.Email, (f) => f.Internet.Email())
            .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLengh));
    }
}