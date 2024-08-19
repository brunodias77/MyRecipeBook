using System.Net;

namespace MRB.Exceptions.Exceptions;


public class InvalidLoginException : MyRecipesBookExceptionBase
{
    public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}