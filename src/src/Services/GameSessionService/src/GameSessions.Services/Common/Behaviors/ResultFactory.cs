using System.Collections.Concurrent;
using System.Reflection;
using CSharpFunctionalExtensions;

namespace GameSessions.Services.Common.Behaviors;

/// <summary>
/// Создаёт <c>Result.Failure&lt;T&gt;</c> через reflection, когда тип
/// <typeparamref name="TResponse"/> известен только в рантайме
/// (внутри open-generic <see cref="ValidationBehavior{TRequest,TResponse}"/>).
/// </summary>
internal static class ResultFactory
{
    private static readonly ConcurrentDictionary<Type, Func<string, object>> _factories = new();

    public static TResponse CreateFailure<TResponse>(string error)
    {
        var factory = _factories.GetOrAdd(typeof(TResponse), BuildFactory);
        return (TResponse)factory(error);
    }

    private static Func<string, object> BuildFactory(Type responseType)
    {
        if (responseType == typeof(Result))
            return error => Result.Failure(error);

        if (responseType == typeof(UnitResult<string>))
            return error => UnitResult.Failure(error);

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = responseType.GetGenericArguments()[0];

            var failureMethod = typeof(Result)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(Result.Failure)
                             && m.IsGenericMethod
                             && m.GetParameters().Length == 1
                             && m.GetParameters()[0].ParameterType == typeof(string))
                .MakeGenericMethod(valueType);

            return error => failureMethod.Invoke(null, new object[] { error })!;
        }

        throw new InvalidOperationException(
            $"ValidationBehavior supports Result / Result<T> / UnitResult<string>, " +
            $"but handler returns '{responseType.Name}'.");
    }
}