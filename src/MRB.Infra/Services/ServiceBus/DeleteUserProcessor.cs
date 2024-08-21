using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace MRB.Infra.Services.ServiceBus
{
    public class DeleteUserProcessor
    {
        private readonly ServiceBusProcessor _processor;
        public DeleteUserProcessor(ServiceBusProcessor processo)
        {
            this._processor = processo;
        }
        public ServiceBusProcessor GetProcessor() => _processor;
    }
}