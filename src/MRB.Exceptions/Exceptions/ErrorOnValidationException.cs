using System.Net;

namespace MRB.Exceptions.Exceptions;

public class ErrorOnValidationException : MyRecipesBookExceptionBase
{
    private readonly IList<string> _errorMessages;

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages;
    }

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    
    
    
    
}