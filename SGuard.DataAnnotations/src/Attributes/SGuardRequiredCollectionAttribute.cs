using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that a collection (array, List, Dictionary, ICollection, IEnumerable) is not null and contains at least one item.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SGuardRequiredCollectionAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRequiredCollectionAttribute"/> class.
    /// </summary>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    public SGuardRequiredCollectionAttribute(Type resourceType, string resourceName) : base(resourceType, resourceName) { }

    /// <summary>
    /// Validates the value of the decorated property to ensure it is a non-empty collection.
    /// </summary>
    /// <param name="value">The value of the decorated property to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
    /// Returns <see cref="ValidationResult.Success"/> if the value is valid; otherwise, a validation error.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        var type = value.GetType();

        if (!type.IsArray)
        {
            return value switch
            {
                System.Collections.ICollection { Count: > 0 } => ValidationResult.Success,
                System.Collections.ICollection                => new ValidationResult(FormatErrorMessage(validationContext.DisplayName)),
                System.Collections.IEnumerable enumerable => enumerable.Cast<object?>().Any()
                                                                 ? ValidationResult.Success
                                                                 : new ValidationResult(FormatErrorMessage(validationContext.DisplayName)),
                _ => ValidationResult.Success
            };
        }

        var array = (Array)value;
        return array.Length > 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}