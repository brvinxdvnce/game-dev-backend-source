using FluentValidation;
using MediatR;

namespace GameSessions.Services.Common.Behaviors;

/// <summary>
/// Конвейерное поведение MediatR: прогоняет каждый запрос через все
/// зарегистрированные <see cref="IValidator"/> для его типа.
/// При наличии ошибок возвращает <c>Result.Failure&lt;T&gt;</c> с агрегированным
/// сообщением, не вызывая обработчик. Исключений не бросает — контракт
/// соответствует <c>CSharpFunctionalExtensions.Result</c>.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var aggregated = string.Join("; ", failures.Select(f =>
            string.IsNullOrEmpty(f.PropertyName)
                ? f.ErrorMessage
                : $"{f.PropertyName}: {f.ErrorMessage}"));

        return ResultFactory.CreateFailure<TResponse>(aggregated);
    }
}