using MRB.Domain.Entities;

namespace MRB.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();

    Task AddAsync(User user);
    Task<bool> ExistActiveUserWithIdentifier(Guid userId);

    Task<bool> ExistActiveUserWithEmail(string email);

    Task<User?> GetByEmailAndPassword(string email, string password);

    Task<User> GetById(Guid id);

    Task<User?> GetUserByEmail(string email);
    
    public void Update(User user);

}