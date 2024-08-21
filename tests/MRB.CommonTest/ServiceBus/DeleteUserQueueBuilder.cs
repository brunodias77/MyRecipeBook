using Moq;
using MRB.Domain.Services.ServiceBus;

namespace MRB.CommonTest.ServiceBus;

public class DeleteUserQueueBuilder
{
    public static IDeleteUserQueue Build()
    {
        return new Mock<IDeleteUserQueue>().Object;
    }
}