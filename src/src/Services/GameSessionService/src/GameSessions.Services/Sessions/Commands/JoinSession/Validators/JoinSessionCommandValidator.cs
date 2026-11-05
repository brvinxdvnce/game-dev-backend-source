namespace GameSessions.Services.Sessions.Commands.JoinSession.Validators; 
using FluentValidation;

public sealed class JoinSessionCommandValidator : AbstractValidator<JoinSessionCommand>
{
    public JoinSessionCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty()
            .WithMessage("SessionId must not be empty.");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Display name is required.")
            .MaximumLength(50)
            .WithMessage("Display name must not exceed 50 characters.")
            .Must(name => name?.Trim().Length > 0)
            .WithMessage("Display name cannot consist of whitespace only.");

        RuleFor(x => x.Team)
            .IsInEnum()
            .WithMessage("Team must be a valid value.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid value.");
    }
}