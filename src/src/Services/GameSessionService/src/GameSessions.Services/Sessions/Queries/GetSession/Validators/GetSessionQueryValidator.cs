using FluentValidation;

namespace GameSessions.Services.Sessions.Queries.GetSession.Validators;

public sealed class GetSessionQueryValidator : AbstractValidator<GetSessionQuery>
{
    public GetSessionQueryValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty()
            .WithMessage("SessionId must not be empty.");
    }
}