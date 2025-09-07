using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// SGuard-localized version of MaxLengthAttribute. This class acts as a wrapper for the sealed <see cref="MaxLengthAttribute"/>
/// to provide support for localized error messages.
/// </summary>
public class SGuardMaxLengthAttribute : MaxLengthAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardMaxLengthAttribute"/> class.
    /// </summary>
    /// <param name="length">The maximum allowable length of the string.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardMaxLengthAttribute(int length, Type resourceType, string resourceName) : base(length)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}