using System.Collections;
using System.Reflection;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Tests.Contracts.Commands;

internal static class BusinessExceptionTestHelpers
{
    internal static IReadOnlyList<object> GetBusinessMessages(BusinessException exception)
    {
        PropertyInfo? messagesProperty = exception
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(
                property => typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                    && property.PropertyType != typeof(string)
                    && IsBusinessExceptionMessageSequence(property, exception));

        if (messagesProperty?.GetValue(exception) is not IEnumerable enumerable)
        {
            return Array.Empty<object>();
        }

        return enumerable.Cast<object>().ToArray();
    }

    internal static IReadOnlyList<string> GetStringValues(object instance)
    {
        return instance
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.PropertyType == typeof(string))
            .Select(property => property.GetValue(instance) as string)
            .Where(value => !string.IsNullOrEmpty(value))
            .Cast<string>()
            .ToArray();
    }

    internal static string? GetBusinessErrorCode(BusinessException exception)
    {
        PropertyInfo[] stringProperties = exception
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.PropertyType == typeof(string))
            .ToArray();

        string? code = stringProperties
            .FirstOrDefault(
                property => property.Name.Contains("Error", StringComparison.OrdinalIgnoreCase)
                    && property.Name.Contains("Code", StringComparison.OrdinalIgnoreCase))
            ?.GetValue(exception) as string;

        if (!string.IsNullOrEmpty(code))
        {
            return code;
        }

        return stringProperties
            .Select(property => property.GetValue(exception) as string)
            .FirstOrDefault(value => value == "E");
    }

    private static bool IsBusinessExceptionMessageSequence(PropertyInfo property, BusinessException exception)
    {
        if (property.GetValue(exception) is not IEnumerable enumerable)
        {
            return false;
        }

        IEnumerator enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext() || enumerator.Current is null)
        {
            return false;
        }

        return enumerator.Current.GetType().Name == "BusinessExceptionMessage";
    }
}

