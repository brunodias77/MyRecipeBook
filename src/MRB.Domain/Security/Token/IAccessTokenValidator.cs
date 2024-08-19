namespace MRB.Domain.Security.Token;

public interface IAccessTokenValidator
{
    public Guid ValidateAndGetUserIdentifier(string token);

}