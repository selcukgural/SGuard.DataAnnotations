using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SGuard.DataAnnotations;

/// <summary>
/// Specifies that a property value must compare to another property's value according to the given comparison type.
/// Supports types implementing IComparable.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public abstract class CompareToAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Gets the name of the other property to compare with.
    /// </summary>
    public string OtherProperty { get; }

    /// <summary>
    /// Gets the type of comparison to perform.
    /// </summary>
    public ComparisonType Comparison { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompareToAttribute"/> class.
    /// </summary>
    /// <param name="otherProperty">The name of the other property to compare with.</param>
    /// <param name="comparison">The type of comparison to perform.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="otherProperty"/> is null.
    /// </exception>
    protected CompareToAttribute(string otherProperty, ComparisonType comparison, Type resourceType, string resourceName) : base(
        resourceType, resourceName)
    {
        OtherProperty = otherProperty ?? throw new ArgumentNullException(nameof(otherProperty));
        Comparison = comparison;
    }

    /// <summary>
    /// Validates the value of the decorated property against the specified other property using the defined comparison type.
    /// </summary>
    /// <param name="value">The value of the decorated property to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
    /// Returns <see cref="ValidationResult.Success"/> if the value is valid; otherwise, a validation error.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo =
            validationContext.ObjectType.GetProperty(OtherProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (otherPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {OtherProperty}");
        }

        var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

        if (value == null || otherValue == null)
        {
            return Comparison switch
            {
                ComparisonType.Equal => Equals(value, otherValue)
                                            ? ValidationResult.Success
                                            : new ValidationResult(FormatErrorMessage(validationContext.DisplayName)),
                ComparisonType.NotEqual => !Equals(value, otherValue)
                                               ? ValidationResult.Success
                                               : new ValidationResult(FormatErrorMessage(validationContext.DisplayName)),
                _ => new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
            };
        }

        var valueType = value.GetType();
        var otherType = otherValue.GetType();

        if (!valueType.IsAssignableFrom(otherType) && !otherType.IsAssignableFrom(valueType))
        {
            return new ValidationResult($"Type mismatch: {validationContext.MemberName} ({valueType.Name}) vs {OtherProperty} ({otherType.Name})");
        }

        if (value is not IComparable comparableValue)
        {
            return new ValidationResult($"{validationContext.MemberName} and {OtherProperty} must implement IComparable.");
        }

        var comparisonResult = comparableValue.CompareTo(otherValue);

        var valid = Comparison switch
        {
            ComparisonType.Equal              => comparisonResult == 0,
            ComparisonType.NotEqual           => comparisonResult != 0,
            ComparisonType.GreaterThan        => comparisonResult > 0,
            ComparisonType.GreaterThanOrEqual => comparisonResult >= 0,
            ComparisonType.LessThan           => comparisonResult < 0,
            ComparisonType.LessThanOrEqual    => comparisonResult <= 0,
            _                                 => throw new ArgumentOutOfRangeException(nameof(Comparison), Comparison, null)
        };

        return valid ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}

/// <summary>
/// Defines the types of comparisons that can be performed by the <see cref="CompareToAttribute"/>.
/// </summary>
public enum ComparisonType
{
    /// <summary>
    /// Indicates that the values must be equal.
    /// </summary>
    Equal,

    /// <summary>
    /// Indicates that the values must not be equal.
    /// </summary>
    NotEqual,

    /// <summary>
    /// Indicates that the value must be greater than the other value.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Indicates that the value must be greater than or equal to the other value.
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Indicates that the value must be less than the other value.
    /// </summary>
    LessThan,

    /// <summary>
    /// Indicates that the value must be less than or equal to the other value.
    /// </summary>
    LessThanOrEqual
}