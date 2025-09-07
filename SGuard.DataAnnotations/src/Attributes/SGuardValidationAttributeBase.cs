using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// Base class for localized validation attributes. Provides a mechanism to assign 
/// resource type and resource name for error messages in a DRY (Don't Repeat Yourself) manner.
/// </summary>
public abstract class SGuardValidationAttributeBase : ValidationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardValidationAttributeBase"/> class.
    /// </summary>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    protected SGuardValidationAttributeBase(Type resourceType, string resourceName)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}