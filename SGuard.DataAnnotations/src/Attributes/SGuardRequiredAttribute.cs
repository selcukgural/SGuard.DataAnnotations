namespace SGuard.DataAnnotations;

/// <summary>
/// A custom validation attribute that enforces a required field constraint
/// with support for localized error messages.
/// </summary>
public class SGuardRequiredAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRequiredAttribute"/> class.
    /// </summary>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <remarks>
    /// This constructor sets the resource type and resource name for the localized error message.
    /// </remarks>
    public SGuardRequiredAttribute(Type resourceType, string resourceName) : base(resourceType, resourceName) { }
}