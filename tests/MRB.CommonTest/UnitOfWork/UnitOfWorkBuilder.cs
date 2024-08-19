using Moq;
using MRB.Domain.Repositories;

namespace MRB.CommonTest.UnitOfWork;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}