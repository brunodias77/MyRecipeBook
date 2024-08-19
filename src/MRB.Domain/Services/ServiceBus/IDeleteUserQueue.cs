using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRB.Domain.Entities;

namespace MRB.Domain.Services.ServiceBus
{
    public interface IDeleteUserQueue
    {
        Task SendMessage(User user);

    }
}