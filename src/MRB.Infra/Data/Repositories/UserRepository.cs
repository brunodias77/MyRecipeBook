using Microsoft.EntityFrameworkCore;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userId)
    {
        return await _context.Users.AnyAsync(user => user.Id.Equals(userId) && user.Active);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
    }

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Active &&
                                                                               user.Email.Equals(email) &&
                                                                               user.Password.Equals(password));
    }

    public async Task<User> GetById(Guid id)
    {
        return await _context
            .Users
            .FirstAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user) => _context.Users.Update(user);
}