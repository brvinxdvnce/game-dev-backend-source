using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using GameSessions.Presentation.DTOs.Sessions;
using GameSessions.Services.Sessions.Commands.CreateSession;
using GameSessions.Services.Sessions.Commands.JoinSession;
using GameSessions.Services.Sessions.Queries.GetSession;
using MediatR;

namespace GameSessions.Presentation.Endpoints;

public static class SessionEndpoints
{
    public static IEndpointRouteBuilder MapSessionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/sessions")
            .WithTags("Sessions");
        
        group.MapPost("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSessionCommand());

            return result.ToCreatedAtRouteHttpResult(
                routeName: "GetSession",
                routeValues: id=> new { id = result.Value });
        });

        group.MapPost("/{id:guid}/participants",
            async (Guid id, JoinSessionRequest request, IMediator mediator) =>
            {
                var command = new JoinSessionCommand(id, request.DisplayName, request.Team, request.Role);
                var result = await mediator.Send(command);
                
                return result.ToOkHttpResult();
            });

        group.MapGet("/{id:guid}",
                async (Guid id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetSessionQuery(id));

                    return result.ToOkHttpResult();
                })
            .WithName("GetSession"); // имя нужно для CreatedAtRoute в POST /

        return app;
    }
}