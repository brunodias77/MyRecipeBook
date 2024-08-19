using System.Net;

namespace MRB.Exceptions.Exceptions;

public abstract class MyRecipesBookExceptionBase : SystemException
{
    protected MyRecipesBookExceptionBase(string message) : base(message)
    {
    }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}