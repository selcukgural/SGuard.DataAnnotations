using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SGuard.DataAnnotations.Internal;

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

    /// <summary>
    /// Validates that the value equals the value of <see cref="CompareAttribute.OtherProperty"/>.
    /// </summary>
    /// <param name="value">The value of the property being validated.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns><see cref="ValidationResult.Success"/> if the values are equal; otherwise, a validation error.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the validated object has no public property named by <see cref="CompareAttribute.OtherProperty"/>.
    /// </exception>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // CompareAttribute reports a missing property as a validation error that names it. Throw instead, like the other
        // SGuard attributes, so the name stays out of the messages a client sees.
        ValidationTargets.GetProperty(this, validationContext, OtherProperty, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        return base.IsValid(value, validationContext);
    }
}