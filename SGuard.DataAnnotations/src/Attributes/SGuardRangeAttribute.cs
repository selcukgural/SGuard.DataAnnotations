using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// A custom validation attribute that validates whether a value falls within a specified range
/// with support for localized error messages.
/// </summary>
public class SGuardRangeAttribute : RangeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRangeAttribute"/> class with integer range values.
    /// </summary>
    /// <param name="minimum">The minimum allowable value.</param>
    /// <param name="maximum">The maximum allowable value.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardRangeAttribute(int minimum, int maximum, Type resourceType, string resourceName) : base(minimum, maximum)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRangeAttribute"/> class with double range values.
    /// </summary>
    /// <param name="minimum">The minimum allowable value.</param>
    /// <param name="maximum">The maximum allowable value.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardRangeAttribute(double minimum, double maximum, Type resourceType, string resourceName) : base(minimum, maximum)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}