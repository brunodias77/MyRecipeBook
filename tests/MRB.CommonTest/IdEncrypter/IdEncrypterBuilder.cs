using Sqids;

namespace MRB.CommonTest.IdEncrypter;

public class IdEncrypterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "aMqDtUodRsSxzpfGIjmCZQTAkvnPuBWXhNwFKeLcJyOVEYibgrlH"
        });
    }
}