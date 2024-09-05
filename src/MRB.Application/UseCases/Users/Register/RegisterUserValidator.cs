using FluentValidation; // Importa o FluentValidation, uma biblioteca para validar objetos complexos.
using MRB.Communication.Requests.Users; // Importa as requisições relacionadas a usuários.
using MRB.Domain.Extensions; // Importa extensões de métodos, provavelmente para o domínio da aplicação.
using MRB.Exceptions; // Importa as exceções personalizadas definidas no projeto.

namespace MRB.Application.UseCases.Users.Register
{
    // Classe que valida a requisição para registrar um usuário, utilizando FluentValidation.
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            // Regra para garantir que o campo "Name" não seja vazio.
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY); // Mensagem de erro caso o nome esteja vazio.

            // Regra para garantir que o campo "Email" não seja vazio.
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY); // Mensagem de erro caso o email esteja vazio.

            // Regra para garantir que a senha tenha pelo menos 6 caracteres.
            RuleFor(user => user.Password.Length)
                .GreaterThanOrEqualTo(6)
                .WithMessage(ResourceMessagesException.PASSWORD_GREATER_THAN_OR_EQUAL_TO); // Mensagem de erro caso a senha seja curta demais.

            // Regra condicional para validar o formato do email se o campo não estiver vazio.
            When(user => user.Email.NotEmpty(), // Verifica se o email não está vazio.
                () =>
                {
                    RuleFor(user => user.Email)
                        .EmailAddress() // Valida se o email tem um formato válido.
                        .WithMessage(ResourceMessagesException.EMAIL_INVALID); // Mensagem de erro caso o email seja inválido.
                });
        }
    }
}
