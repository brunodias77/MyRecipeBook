namespace MRB.Communication.Responses;

public class ResponseErrorJson
{
    public IList<string> Errors { get; set; }
    public bool TokenExpired { get; set; }
    public string? Error { get; }

    public ResponseErrorJson(IList<string> errors) => Errors = errors!;

    public ResponseErrorJson(string error)
    {
        Errors = new List<string>
        {
            error!
        };
    }

}