using MRB.Domain.Entities;

namespace MRB.Domain.Services;

public interface ILoggedUser
{
    public Task<User> User();
}