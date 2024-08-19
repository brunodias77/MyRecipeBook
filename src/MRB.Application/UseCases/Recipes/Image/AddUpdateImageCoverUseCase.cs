using Microsoft.AspNetCore.Http;
using MRB.Application.Extensions;
using MRB.Application.UseCases.Recipes.Image;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Domain.Services.Storage;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Image
{
    public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
    {
        private readonly ILoggedUser _loggedUser; // Serviço para obter o usuário logado

        private readonly IRecipeRepository _repository; // Repositório para operações de atualização de receitas

        private readonly IUnitOfWork _unitOfWork; // Unidade de trabalho para garantir transações atômicas
        private readonly IBlobStorageService _blobStorageService; // Serviço de armazenamento de blobs

        public AddUpdateImageCoverUseCase(
            ILoggedUser loggedUser,
            IRecipeRepository repository,
            IUnitOfWork unitOfWork,
            IBlobStorageService blobStorageService)
        {
            _repository = repository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Executa a adição ou atualização da imagem de capa de uma receita.
        /// </summary>
        /// <param name="recipeId">O ID da receita a ser atualizada.</param>
        /// <param name="file">O arquivo de imagem enviado.</param>
        public async Task Execute(Guid recipeId, IFormFile file)
        {
            // Obtém o usuário logado
            var loggedUser = await _loggedUser.User();

            // Obtém a receita pelo ID
            var recipe = await _repository.GetById(loggedUser, recipeId);

            // Verifica se a receita existe
            if (recipe is null)
                throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

            // Abre um fluxo de leitura para o arquivo enviado
            var fileStream = file.OpenReadStream();

            // Valida se o arquivo é uma imagem e obtém a extensão
            (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

            // Verifica se o arquivo é uma imagem válida
            if (!isValidImage)
            {
                throw new ErrorOnValidationException(new[] { ResourceMessagesException.ONLY_IMAGES_ACCEPTED });
            }

            // Se a receita não possui uma imagem, cria um identificador para a imagem
            if (string.IsNullOrEmpty(recipe.ImageIdentifier))
            {
                recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

                // Atualiza a receita no repositório
                _repository.Update(recipe);

                // Confirma a transação
                await _unitOfWork.Commit();
            }

            // Faz o upload da imagem para o serviço de armazenamento no azure
            await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
        }
    }
}