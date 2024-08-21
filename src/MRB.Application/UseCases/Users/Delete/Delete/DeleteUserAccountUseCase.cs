using MRB.Domain.Repositories;
using MRB.Domain.Services.Storage;

namespace MRB.Application.UseCases.Users.Delete.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    public DeleteUserAccountUseCase(IUserRepository repository, IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public async Task Execute(Guid userId)
    {
        await _blobStorageService.DeleteContainer(userId);
        await _repository.DeleteAccount(userId);
        await _unitOfWork.Commit();
    }
}