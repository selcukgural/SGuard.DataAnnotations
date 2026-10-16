using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SGuard.DataAnnotations.Internal;

/// <summary>
/// Lookups shared by the attributes that read other properties of the model or the items of a collection. Mistakes in
/// how an attribute is declared throw instead of becoming validation errors, so that the details stay in the server's
/// logs and never reach a client as a validation message.
/// </summary>
internal static class ValidationTargets
{
    /// <summary>
    /// Returns the property <paramref name="propertyName"/> of the validated object.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the object's type has no such property.</exception>
    public static PropertyInfo GetProperty(ValidationAttribute attribute, ValidationContext validationContext, string propertyName,
                                           BindingFlags flags)
    {
        return validationContext.ObjectType.GetProperty(propertyName, flags) ??
               throw new InvalidOperationException(
                   $"{attribute.GetType().Name} on '{validationContext.ObjectType.FullName}.{validationContext.MemberName}' refers to " +
                   $"the property '{propertyName}', which the type doesn't have.");
    }

    /// <summary>
    /// Throws when <paramref name="value"/> is an <see cref="IQueryable"/>: counting or enumerating it would run the query
    /// against its data source during validation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="value"/> is an <see cref="IQueryable"/>.</exception>
    public static void ThrowIfQueryable(ValidationAttribute attribute, object value)
    {
        if (value is IQueryable)
        {
            throw new InvalidOperationException(
                $"{attribute.GetType().Name} doesn't validate IQueryable values, because enumerating them runs the query. " +
                "Validate a materialized collection instead, for example the result of ToList().");
        }
    }
}
