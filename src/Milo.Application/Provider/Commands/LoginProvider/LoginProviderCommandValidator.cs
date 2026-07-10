using FluentValidation;

namespace Milo.Application.Provider.Commands.LoginProvider;

public sealed class LoginProviderCommandValidator : AbstractValidator<LoginProviderCommand>
{
    public LoginProviderCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}