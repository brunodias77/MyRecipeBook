using Azure.Messaging.ServiceBus;
using MRB.Domain.Entities;
using MRB.Domain.Services.ServiceBus;
namespace MRB.Infra.Services.ServiceBus
{
    public class DeleteUserQueue : IDeleteUserQueue
    {

        private readonly ServiceBusSender _serviceBusSender;

        public DeleteUserQueue(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        public async Task SendMessage(User user)
        {
            await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.Id.ToString()));
        }
    }
}