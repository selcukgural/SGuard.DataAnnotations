using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that a collection (array, List, Dictionary, ICollection, IEnumerable) contains at least a minimum number of items.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SGuardMinCountAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Gets the minimum allowed number of items in the collection.
    /// </summary>
    public int MinCount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardMinCountAttribute"/> class.
    /// </summary>
    /// <param name="minCount">The minimum number of items required in the collection.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="minCount"/> is less than 0.
    /// </exception>
    public SGuardMinCountAttribute(int minCount, Type resourceType, string resourceName) : base(resourceType, resourceName)
    {
        if (minCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minCount), @"Minimum count cannot be negative.");
        }

        MinCount = minCount;
    }

    /// <summary>
    /// Validates the value of the decorated property to ensure it meets the minimum count requirement.
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
            return MinCount == 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        var type = value.GetType();

        // Array
        if (type.IsArray)
        {
            var array = (Array)value;
            return array.Length >= MinCount ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        switch (value)
        {
            // ICollection (includes List, Dictionary, etc.)
            case System.Collections.ICollection coll:
                return coll.Count >= MinCount ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            // IEnumerable fallback (for custom collection types)
            case System.Collections.IEnumerable enumerable:
            {
                var count = 0;

                foreach (var _ in enumerable)
                {
                    count++;

                    if (count >= MinCount)
                    {
                        return ValidationResult.Success;
                    }
                }

                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            default:
                // Not a collection: always valid
                return ValidationResult.Success;
        }
    }
}