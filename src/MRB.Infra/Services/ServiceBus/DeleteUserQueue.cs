using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task SendMessage(User user)
        {
            throw new NotImplementedException();
        }
    }
}