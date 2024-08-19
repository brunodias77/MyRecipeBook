namespace MRB.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();

}