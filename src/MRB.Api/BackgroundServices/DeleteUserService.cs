using Azure.Messaging.ServiceBus; // Importa a biblioteca para comunicação com o Service Bus da Azure
using MRB.Application.UseCases.Users.Delete.Delete; // Importa o caso de uso para deletar um usuário
using MRB.Infra.Services.ServiceBus; // Importa os serviços do Service Bus

namespace MRB.Api.BackgroundServices
{
    // Define um serviço em segundo plano para processar mensagens do Service Bus
    public class DeleteUserService : BackgroundService
    {
        private readonly IServiceProvider _services; // Provedor de serviços para resolver dependências
        private readonly ServiceBusProcessor _processor; // Processador para mensagens do Service Bus

        // Construtor que recebe o provedor de serviços e o processador
        public DeleteUserService(IServiceProvider services, DeleteUserProcessor processor)
        {
            _processor = processor.GetProcessor(); // Obtém o processador a partir do serviço
            _services = services; // Armazena o provedor de serviços
        }

        // Método assíncrono que é executado quando o serviço é iniciado
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Associa os métodos de tratamento de mensagens e erros ao processador
            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ExceptionReceivedHandler;

            // Inicia o processamento das mensagens do Service Bus
            await _processor.StartProcessingAsync(stoppingToken);
        }

        // Método que processa as mensagens recebidas do Service Bus
        private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
        {
            // Obtém o corpo da mensagem e converte para string
            var message = eventArgs.Message.Body.ToString();

            // Faz o parsing da mensagem para um identificador de usuário (GUID)
            var userIdentifier = Guid.Parse(message);

            // Cria um novo escopo de serviços para resolver a instância do caso de uso
            var scope = _services.CreateScope();

            // Obtém a instância do caso de uso para deletar um usuário a partir do escopo de serviços
            var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

            // Executa o caso de uso para deletar o usuário
            await deleteUserUseCase.Execute(userIdentifier);
        }

        // Método que lida com erros recebidos durante o processamento das mensagens
        private Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;
    }
}