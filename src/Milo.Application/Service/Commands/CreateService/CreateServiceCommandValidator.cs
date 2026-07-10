using FluentValidation;

namespace Milo.Application.Service.Commands.CreateService;

public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CostPerHour)
            .GreaterThan(0);

        RuleFor(x => x.ProviderId)
            .NotEmpty();
    }
}