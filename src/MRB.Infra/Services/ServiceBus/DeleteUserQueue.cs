using Azure.Messaging.ServiceBus;
using MRB.Domain.Entities;
using MRB.Domain.Services.ServiceBus;

namespace MRB.Infra.Services.ServiceBus
{
    // Implementa a interface IDeleteUserQueue e é responsável por enviar mensagens relacionadas à exclusão de usuários para a fila do Service Bus.
    public class DeleteUserQueue : IDeleteUserQueue
    {
        // Instância do ServiceBusSender usada para enviar mensagens para o Service Bus.
        private readonly ServiceBusSender _serviceBusSender;

        // Construtor que recebe um ServiceBusSender e inicializa o campo _serviceBusSender.
        public DeleteUserQueue(ServiceBusSender serviceBusSender)
        {
            // Inicializa o campo _serviceBusSender com a instância recebida.
            _serviceBusSender = serviceBusSender;
        }

        // Envia uma mensagem para a fila do Service Bus com o ID do usuário.
        public async Task SendMessage(User user)
        {
            // Cria uma nova mensagem com o ID do usuário convertido para string.
            var message = new ServiceBusMessage(user.Id.ToString());

            // Envia a mensagem de forma assíncrona para a fila do Service Bus.
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}