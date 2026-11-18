using System.Collections;
using System.Reflection;
using Fxfer.DomainApi.Api;
using Fxfer.DomainApi.Contracts.Exceptions;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Api.Middleware;

public sealed class ApiExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BusinessException ex)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await FxferProblemDetails.WriteAsync(
                context,
                statusCode: StatusCodes.Status400BadRequest,
                title: ex.Message,
                errors: MapValidationErrors(ex));

            return;
        }
        catch (FxferAuthorizationException ex)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await FxferProblemDetails.WriteAsync(
                context,
                statusCode: StatusCodes.Status403Forbidden,
                title: ex.Message);

            return;
        }
        catch
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await FxferProblemDetails.WriteAsync(
                context,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unhandled error");

            return;
        }

        if (context.Response.HasStarted)
        {
            return;
        }

        if (context.Response.StatusCode < 400)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(context.Response.ContentType))
        {
            return;
        }

        await FxferProblemDetails.WriteAsync(
            context,
            statusCode: context.Response.StatusCode,
            title: "Request failed");
    }

    private static IReadOnlyList<ValidationError>? MapValidationErrors(BusinessException ex)
    {
        var messagesProperty = ex
            .GetType()
            .GetProperties()
            .FirstOrDefault(property => typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                && property.PropertyType != typeof(string)
                && IsBusinessExceptionMessageSequence(property.GetValue(ex) as IEnumerable));

        if (messagesProperty?.GetValue(ex) is not IEnumerable messages)
        {
            return null;
        }

        var errors = new List<ValidationError>();

        foreach (var message in messages)
        {
            if (message is null)
            {
                continue;
            }

            var type = message.GetType();

            var stringProperties = type
                .GetProperties()
                .Where(property => property.PropertyType == typeof(string))
                .ToArray();

            static string? GetStringValue(object instance, PropertyInfo property)
                => property.GetValue(instance) as string;

            var fieldProperty = stringProperties.FirstOrDefault(property => property.Name.Contains("Key", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("Name", StringComparison.OrdinalIgnoreCase));

            var values = stringProperties
                .Select(property => GetStringValue(message, property))
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Cast<string>()
                .ToArray();

            var field = (fieldProperty is null ? null : GetStringValue(message, fieldProperty))
                        ?? values.FirstOrDefault()
                        ?? string.Empty;

            var errorMessage = values.FirstOrDefault(value => !string.Equals(value, field, StringComparison.Ordinal))
                               ?? values.LastOrDefault()
                               ?? string.Empty;

            errors.Add(new ValidationError
            {
                Field = field,
                Code = field,
                Message = errorMessage
            });
        }

        return errors.Count == 0 ? null : errors;
    }


    private static bool IsBusinessExceptionMessageSequence(IEnumerable? enumerable)
    {
        if (enumerable is null)
        {
            return false;
        }

        var enumerator = enumerable.GetEnumerator();
        if (!enumerator.MoveNext() || enumerator.Current is null)
        {
            return false;
        }

        return enumerator.Current.GetType().Name == "BusinessExceptionMessage";
    }
}
