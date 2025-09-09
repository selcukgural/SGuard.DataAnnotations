using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that all items in a collection match the specified validation attribute(s).
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SGuardCollectionItemsMatchAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Gets the validation attribute types to apply to each item in the collection.
    /// </summary>
    public Type[] ItemValidationAttributes { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to collect all errors in the collection.
    /// If set to false, validation stops at the first error. Default is false.
    /// </summary>
    public bool AggregateAllErrors { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardCollectionItemsMatchAttribute"/> class
    /// for a single validation attribute.
    /// </summary>
    /// <param name="itemValidationAttribute">The validation attribute type to apply to each item.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="itemValidationAttribute"/>, <paramref name="resourceType"/>, or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardCollectionItemsMatchAttribute(Type itemValidationAttribute, Type resourceType, string resourceName) : base(
        resourceType, resourceName)
    {
        ItemValidationAttributes = new[] { itemValidationAttribute ?? throw new ArgumentNullException(nameof(itemValidationAttribute)) };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardCollectionItemsMatchAttribute"/> class
    /// for multiple validation attributes.
    /// </summary>
    /// <param name="itemValidationAttributes">An array of validation attribute types to apply to each item.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="itemValidationAttributes"/>, <paramref name="resourceType"/>, or <paramref name="resourceName"/> is null.
    /// </exception>
    public SGuardCollectionItemsMatchAttribute(Type[] itemValidationAttributes, Type resourceType, string resourceName) : base(
        resourceType, resourceName)
    {
        ItemValidationAttributes = itemValidationAttributes ?? throw new ArgumentNullException(nameof(itemValidationAttributes));
    }

    /// <summary>
    /// Validates the collection to ensure all items match the specified validation attribute(s).
    /// </summary>
    /// <param name="value">The value of the property or field being validated.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether validation succeeded or failed.
    /// Returns <see cref="ValidationResult.Success"/> if validation passes, or an error result otherwise.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not System.Collections.IEnumerable enumerable)
        {
            return ValidationResult.Success;
        }

        var errors = new List<ValidationResult>();
        var idx = 0;

        foreach (var item in enumerable)
        {
            foreach (var attrType in ItemValidationAttributes)
            {
                if (Activator.CreateInstance(attrType) is not ValidationAttribute attr)
                {
                    continue;
                }

                attr.ErrorMessageResourceType = ErrorMessageResourceType;
                attr.ErrorMessageResourceName = ErrorMessageResourceName;

                if (attr.IsValid(item))
                {
                    continue;
                }

                if (!AggregateAllErrors)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                                                ReturnMemberNameList(validationContext.MemberName));
                }

                var msg = $"{FormatErrorMessage(validationContext.DisplayName)} (item #{idx + 1})";
                errors.Add(new ValidationResult(msg, ReturnMemberNameList(validationContext.MemberName)));
            }

            idx++;
        }

        if (errors.Count == 0)
        {
            return ValidationResult.Success;
        }

        var merged = string.Join("; ", errors.Select(e => e.ErrorMessage));
        return new ValidationResult(merged, ReturnMemberNameList(validationContext.MemberName ?? "item"));
    }

    /// <summary>
    /// Returns a list containing the member name for the validation result.
    /// </summary>
    /// <param name="memberName">The name of the member being validated.</param>
    /// <returns>A list containing the member name.</returns>
    private static List<string> ReturnMemberNameList(string? memberName)
    {
        return new List<string>
        {
            memberName ?? "item"
        };
    }
}