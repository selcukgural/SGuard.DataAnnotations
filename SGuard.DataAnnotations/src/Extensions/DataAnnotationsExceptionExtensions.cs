using System.Collections;
using SGuard.DataAnnotations.Exceptions;

namespace SGuard.DataAnnotations.Extensions;

/// <summary>
/// Provides extension methods for handling <see cref="DataAnnotationsException"/>.
/// </summary>
public static class DataAnnotationsExceptionExtensions
{
    /// <summary>
    /// Attempts to retrieve validation errors from a <see cref="DataAnnotationsException"/>.
    /// </summary>
    /// <param name="exception">The <see cref="DataAnnotationsException"/> instance to extract validation errors from.</param>
    /// <param name="errors">
    /// When this method returns, contains the collection of <see cref="ValidationError"/> objects if found; 
    /// otherwise, an empty collection.
    /// </param>
    /// <returns>
    /// <c>true</c> if validation errors were successfully retrieved; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="exception"/> is <c>null</c>.</exception>
    public static bool TryGetValidationErrors(this DataAnnotationsException exception, out List<ValidationError> errors)
    {
        ArgumentNullException.ThrowIfNull(exception);

        errors = new List<ValidationError>();

        if (exception.Data.Count == 0)
        {
            return false;
        }

        foreach (DictionaryEntry entry in exception.Data)
        {
            if (entry.Key is not DataAnnotationsException.DataKey || entry.Value is not List<ValidationError> errorsList)
            {
                continue;
            }

            errors.AddRange(errorsList);
            return true;
        }

        return false;
    }
}