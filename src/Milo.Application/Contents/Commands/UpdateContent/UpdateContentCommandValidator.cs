using FluentValidation;
using Milo.Domain.Entities.Enums;

namespace Milo.Application.Contents.Commands.UpdateContent;

public sealed class UpdateContentCommandValidator : AbstractValidator<UpdateContentCommand>
{
    public UpdateContentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
        RuleFor(x => x.Type)
            .Must(t => Enum.TryParse<ContentType>(t, ignoreCase: true, out _))
            .WithMessage("El tipo debe ser 'Image', 'Video' o 'Text'");
    }
}