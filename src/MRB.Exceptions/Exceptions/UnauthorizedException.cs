using System.Net;

namespace MRB.Exceptions.Exceptions;

public class UnauthorizedException : MyRecipesBookExceptionBase
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.Unauthorized;
    }
}