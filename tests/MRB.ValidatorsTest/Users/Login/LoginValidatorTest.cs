using FluentAssertions;
using MRB.Application.UseCases.Users.Login;
using MRB.CommonTest.Requests.Users;
using MRB.Exceptions;
using Xunit;

namespace MRB.ValidatorsTest.Users.Login;

public class LoginValidatorTest
{
    [Fact]
    public void SUCESSO()
    {
        var validator = new LoginUserValidator();

        var request = RequestLoginJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().Be(true);
    }

    [Fact]
    public void ERRO_EMAIL_VAZIO()
    {
        var validator = new LoginUserValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public void ERRO_SENHA_VAZIA()
    {
        var validator = new LoginUserValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public void ERRO_EMAIL_INVALIDO()
    {
        var validator = new LoginUserValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = "invalidEmail";

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public void ERROR_SENHA_INVALIDA()
    {
        var validator = new LoginUserValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Password = "1234";

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }
}