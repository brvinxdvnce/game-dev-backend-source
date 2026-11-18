using FluentValidation;
using GameSessions.Infrastructure;
using GameSessions.Presentation.Endpoints;
using GameSessions.Services.Common.Behaviors;
using GameSessions.Services.Sessions.Commands.CreateSession;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// TODO нормальное подключение, а не хакдкод
builder.Services.AddDbContext<SessionsDbContext>(options => 
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateSessionCommand>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateSessionCommand>();

var app = builder.Build();

app.MapSessionEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();


