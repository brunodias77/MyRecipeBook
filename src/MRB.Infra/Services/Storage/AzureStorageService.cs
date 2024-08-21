using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MRB.Domain.Entities;
using MRB.Domain.Services.Storage;
using MRB.Domain.ValueObjects;

namespace MRB.Infra.Services.Storage
{
    // Serviço que implementa a interface IBlobStorageService e lida com operações no Azure Blob Storage.
    public class AzureStorageService : IBlobStorageService
    {
        // Cliente do BlobService usado para interagir com o Azure Blob Storage.
        private readonly BlobServiceClient _blobServiceClient;

        // Construtor que inicializa o BlobServiceClient.
        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        // Método para fazer upload de um arquivo para o contêiner de um usuário.
        public async Task Upload(User user, Stream file, string fileName)
        {
            // Obtém o contêiner correspondente ao ID do usuário.
            var container = _blobServiceClient.GetBlobContainerClient(user.Id.ToString());

            // Cria o contêiner se ele ainda não existir.
            await container.CreateIfNotExistsAsync();

            // Obtém o cliente do Blob para o arquivo especificado.
            var blobClient = container.GetBlobClient(fileName);

            // Faz upload do arquivo, sobrescrevendo o existente, se houver.
            await blobClient.UploadAsync(file, overwrite: true);
        }

        // Método para obter a URL de um arquivo com uma assinatura SAS temporária.
        public async Task<string> GetFileUrl(User user, string fileName)
        {
            var containerName = user.Id.ToString();

            // Obtém o cliente do contêiner e verifica se ele existe.
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var exist = await containerClient.ExistsAsync();
            if (!exist.Value)
                return string.Empty;

            // Obtém o cliente do Blob para o arquivo especificado e verifica se o Blob existe.
            var blobClient = containerClient.GetBlobClient(fileName);
            exist = await blobClient.ExistsAsync();
            if (exist.Value)
            {
                // Cria um SAS (Shared Access Signature) com permissão de leitura que expira após um tempo configurado.
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b", // Tipo de recurso Blob.
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MyRecipesBookRuleConstants
                        .MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES), // Tempo de vida da URL.
                };

                // Define permissões de leitura para o SAS.
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Gera a URL com o token SAS e a retorna como string.
                return blobClient.GenerateSasUri(sasBuilder).ToString();
            }

            return string.Empty;
        }

        // Método para deletar um arquivo de um contêiner específico.
        public async Task Delete(User user, string fileName)
        {
            // Obtém o cliente do contêiner do usuário e verifica se ele existe.
            var containerClient = _blobServiceClient.GetBlobContainerClient(user.Id.ToString());
            var exists = await containerClient.ExistsAsync();
            if (exists.Value)
            {
                // Deleta o Blob especificado, se ele existir.
                await containerClient.DeleteBlobIfExistsAsync(fileName);
            }
        }

        // Método para deletar um contêiner inteiro, baseado no ID do usuário.
        public async Task DeleteContainer(Guid userId)
        {
            // Obtém o cliente do contêiner e deleta o contêiner, se ele existir.
            var container = _blobServiceClient.GetBlobContainerClient(userId.ToString());
            await container.DeleteIfExistsAsync();
        }
    }
}