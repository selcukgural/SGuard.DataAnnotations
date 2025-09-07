using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// A custom validation attribute for validating email addresses with support for localized error messages.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class SGuardEmailAddressAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardEmailAddressAttribute"/> class.
    /// </summary>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardEmailAddressAttribute(Type resourceType, string resourceName) : base(resourceType, resourceName) { }

    /// <summary>
    /// Validates the specified value with the context of the validation.
    /// </summary>
    /// <param name="value">The value of the object to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
    /// Returns <see cref="ValidationResult.Success"/> if the value is valid; otherwise, a validation error.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var inner = new EmailAddressAttribute
        {
            ErrorMessageResourceType = ErrorMessageResourceType,
            ErrorMessageResourceName = ErrorMessageResourceName
        };

        return inner.IsValid(value) ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}