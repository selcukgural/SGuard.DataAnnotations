using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that a collection (array, List, Dictionary, ICollection, IEnumerable) contains at most a maximum number of items.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SGuardMaxCountAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Gets the maximum allowed number of items in the collection.
    /// </summary>
    public int MaxCount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardMaxCountAttribute"/> class.
    /// </summary>
    /// <param name="maxCount">The maximum number of items allowed in the collection.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="maxCount"/> is less than 0.
    /// </exception>
    public SGuardMaxCountAttribute(int maxCount, Type resourceType, string resourceName) : base(resourceType, resourceName)
    {
        if (maxCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCount), @"Maximum count cannot be negative.");
        }

        MaxCount = maxCount;
    }

    /// <summary>
    /// Validates the value of the decorated property to ensure it does not exceed the maximum count.
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
            return ValidationResult.Success;
        }

        var type = value.GetType();

        // Array
        if (type.IsArray)
        {
            var array = (Array)value;
            return array.Length <= MaxCount ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        switch (value)
        {
            case System.Collections.ICollection coll:
            {
                return coll.Count <= MaxCount ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            case System.Collections.IEnumerable enumerable:
            {
                var count = 0;

                foreach (var _ in enumerable)
                {
                    count++;

                    if (count > MaxCount)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }

                return ValidationResult.Success;
            }
            default:
                // Not a collection: always valid
                return ValidationResult.Success;
        }
    }
}