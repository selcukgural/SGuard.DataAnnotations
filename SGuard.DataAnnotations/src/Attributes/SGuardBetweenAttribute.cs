using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that the decorated property's value is between two other property values (inclusive or exclusive).
/// </summary>
/// <remarks>
/// This attribute compares the value of the decorated property with the values of two other properties
/// (specified by <see cref="MinProperty"/> and <see cref="MaxProperty"/>). The comparison can be inclusive
/// or exclusive based on the <see cref="Inclusive"/> property.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class SGuardBetweenAttribute : SGuardValidationAttributeBase
{
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// Gets the name of the property representing the minimum value.
    /// </summary>
    public string MinProperty { get; }

    /// <summary>
    /// Gets the name of the property representing the maximum value.
    /// </summary>
    public string MaxProperty { get; }

    /// <summary>
    /// Gets a value indicating whether the comparison is inclusive.
    /// </summary>
    public bool Inclusive { get; }

    /// <summary>
    /// Creates a new instance of <see cref="SGuardBetweenAttribute"/>.
    /// </summary>
    /// <param name="minProperty">Name of the property representing the minimum value.</param>
    /// <param name="maxProperty">Name of the property representing the maximum value.</param>
    /// <param name="inclusive">If true, the comparison is inclusive (default: true).</param>
    /// <param name="resourceType">The resource type for the error message.</param>
    /// <param name="resourceName">The resource name for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="minProperty"/> or <paramref name="maxProperty"/> is null.
    /// </exception>
    public SGuardBetweenAttribute(string minProperty, string maxProperty, bool inclusive, Type resourceType, string resourceName) : base(
        resourceType, resourceName)
    {
        MinProperty = minProperty ?? throw new ArgumentNullException(nameof(minProperty));
        MaxProperty = maxProperty ?? throw new ArgumentNullException(nameof(maxProperty));
        Inclusive = inclusive;
    }

    /// <summary>
    /// Validates the value of the decorated property against the specified minimum and maximum properties.
    /// </summary>
    /// <param name="value">The value of the decorated property to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
    /// Returns <see cref="ValidationResult.Success"/> if the value is valid; otherwise, a validation error.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var objType = validationContext.ObjectType;

        var minPropInfo = objType.GetProperty(MinProperty, Flags);
        var maxPropInfo = objType.GetProperty(MaxProperty, Flags);

        if (minPropInfo == null)
        {
            return new ValidationResult($"Unknown property: {MinProperty}");
        }

        if (maxPropInfo == null)
        {
            return new ValidationResult($"Unknown property: {MaxProperty}");
        }

        var minValue = minPropInfo.GetValue(validationContext.ObjectInstance, null);
        var maxValue = maxPropInfo.GetValue(validationContext.ObjectInstance, null);

        if (value == null || minValue == null || maxValue == null)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        var valueType = value.GetType();
        var minType = minValue.GetType();
        var maxType = maxValue.GetType();

        // Check type compatibility
        if (!valueType.IsAssignableFrom(minType) && !minType.IsAssignableFrom(valueType))
        {
            return new ValidationResult($"Type mismatch: {validationContext.MemberName} vs {MinProperty}");
        }

        if (!valueType.IsAssignableFrom(maxType) && !maxType.IsAssignableFrom(valueType))
        {
            return new ValidationResult($"Type mismatch: {validationContext.MemberName} vs {MaxProperty}");
        }

        if (value is IComparable cmpValue)
        {
            var minResult = cmpValue.CompareTo(minValue);
            var maxResult = cmpValue.CompareTo(maxValue);

            var valid = Inclusive ? minResult >= 0 && maxResult <= 0 : minResult > 0 && maxResult < 0;

            return valid ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return new ValidationResult($"{validationContext.MemberName}, {MinProperty}, and {MaxProperty} must implement IComparable.");
    }
}