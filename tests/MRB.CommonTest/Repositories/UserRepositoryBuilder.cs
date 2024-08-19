using Moq;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.CommonTest.Repositories;

public class UserRepositoryBuilder
{
    private readonly Mock<IUserRepository> repository;

    public UserRepositoryBuilder()
    {
        repository = new Mock<IUserRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        repository.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmailAndPassword(User user)
    {
        repository.Setup(repo => repo.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
    }

    public UserRepositoryBuilder GetById(User user)
    {
        repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public UserRepositoryBuilder GetUserByEmail(User user)
    {
        repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }


    public IUserRepository Build()
    {
        return repository.Object;
    }
}