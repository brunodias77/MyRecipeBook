using MRB.Domain.Entities;

namespace MRB.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task Upload(User user, Stream file, string fileName);
    Task<string> GetFileUrl(User user, string fileName);
    Task Delete(User user, string fileName);
    Task DeleteContainer(Guid userIdentifier);
}