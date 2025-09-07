using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// A custom attribute that extends the <see cref="StringLengthAttribute"/> to support localization.
/// </summary>
public class SGuardStringLengthAttribute : StringLengthAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardStringLengthAttribute"/> class.
    /// </summary>
    /// <param name="maximumLength">The maximum allowable length of the string.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardStringLengthAttribute(int maximumLength, Type resourceType, string resourceName) : base(maximumLength)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}