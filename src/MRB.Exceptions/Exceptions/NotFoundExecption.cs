using System.Net;

namespace MRB.Exceptions.Exceptions;



public class NotFoundException : MyRecipesBookExceptionBase
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}