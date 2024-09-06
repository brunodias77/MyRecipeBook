using Azure.Messaging.ServiceBus;

namespace MRB.Infra.Services.ServiceBus
{
    // Classe responsável por processar mensagens do Service Bus relacionadas à exclusão de usuários.
    public class DeleteUserProcessor
    {
        // Instância do ServiceBusProcessor que será usada para processar mensagens.
        private readonly ServiceBusProcessor _processor;

        // Construtor que recebe um ServiceBusProcessor e inicializa o campo _processor.
        public DeleteUserProcessor(ServiceBusProcessor processo)
        {
            // Inicializa o campo _processor com a instância recebida.
            this._processor = processo;
        }

        // Método que retorna a instância do ServiceBusProcessor.
        public ServiceBusProcessor GetProcessor() => _processor;
    }
}