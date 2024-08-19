using Moq;
using MRB.Domain.Security;

namespace MRB.CommonTest.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        public readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncrypterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            // Define o comportamento padrão do método Encrypt para retornar uma string específica
            _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!%dlfjkd545");
        }

        public PasswordEncrypterBuilder Verify(string? password)
        {
            // Verifica se a senha fornecida não é nula ou vazia
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                // Define o comportamento do método Verify para retornar true quando a senha corresponde
                _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);
            }

            // Retorna a instância atual para permitir encadeamento de chamadas
            return this;
        }

        public IPasswordEncripter Build()
        {
            return _mock.Object;
        }
    }
}