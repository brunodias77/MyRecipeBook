namespace MRB.Communication.Responses.Users;

public class ResponseRegisterUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokenJson Tokens { get; set; } = default!;
}