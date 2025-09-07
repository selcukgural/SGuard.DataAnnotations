using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// SGuard-localized version of CompareAttribute. This class acts as a wrapper for the sealed <see cref="CompareAttribute"/>
/// to provide support for localized error messages.
/// </summary>
public class SGuardCompareAttribute : CompareAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardCompareAttribute"/> class.
    /// </summary>
    /// <param name="otherProperty">The name of the other property to compare with.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardCompareAttribute(string otherProperty, Type resourceType, string resourceName) : base(otherProperty)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}