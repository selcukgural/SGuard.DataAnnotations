using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// A custom validation attribute that validates a value against a regular expression pattern
/// with support for localized error messages.
/// </summary>
public class SGuardRegularExpressionAttribute : RegularExpressionAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRegularExpressionAttribute"/> class.
    /// </summary>
    /// <param name="pattern">The regular expression pattern to validate the value against.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardRegularExpressionAttribute(string pattern, Type resourceType, string resourceName)
        : base(pattern)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}