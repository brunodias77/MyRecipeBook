using Moq;
using MRB.Domain.Entities;
using MRB.Domain.Services;

namespace MRB.CommonTest.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();
        mock.Setup(x => x.User()).ReturnsAsync(user);
        return mock.Object;
    }
}