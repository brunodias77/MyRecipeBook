namespace MRB.Application.UseCases.Users.Delete.Delete;

public interface IDeleteUserAccountUseCase
{
    Task Execute(Guid userId);
}