using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SGuard.DataAnnotations;

/// <summary>
/// Requires the property to have a value if another property equals a specific value (or set of values).
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class SGuardRequiredIfAttribute : SGuardValidationAttributeBase
{
    /// <summary>
    /// Gets a value indicating whether the condition should be inverted.
    /// </summary>
    public bool Invert { get; }

    /// <summary>
    /// Gets the name of the dependent property whose value is used in the condition.
    /// </summary>
    public string DependentProperty { get; }

    /// <summary>
    /// Gets the expected value of the dependent property that triggers the requirement.
    /// </summary>
    public object? ExpectedValue { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardRequiredIfAttribute"/> class.
    /// </summary>
    /// <param name="dependentProperty">The name of the dependent property.</param>
    /// <param name="expectedValue">The value of the dependent property that triggers the requirement.</param>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <param name="invert">Indicates whether to invert the condition (optional).</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="dependentProperty"/> is null.
    /// </exception>
    public SGuardRequiredIfAttribute(string dependentProperty, object? expectedValue, Type resourceType, string resourceName, bool invert = false) :
        base(resourceType, resourceName)
    {
        Invert = invert;
        ExpectedValue = expectedValue;
        DependentProperty = dependentProperty ?? throw new ArgumentNullException(nameof(dependentProperty));
    }

    /// <summary>
    /// Validates the value of the decorated property based on the condition defined by the dependent property and its expected value.
    /// </summary>
    /// <param name="value">The value of the decorated property to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
    /// Returns <see cref="ValidationResult.Success"/> if the value is valid; otherwise, a validation error.
    /// </returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(DependentProperty,
                                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        if (property == null)
        {
            return new ValidationResult($"Unknown property: {DependentProperty}");
        }

        var dependentValue = property.GetValue(validationContext.ObjectInstance, null);

        var conditionMet = Equals(dependentValue, ExpectedValue);
        if (Invert) conditionMet = !conditionMet;

        if (!conditionMet)
        {
            return ValidationResult.Success;
        }

        if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}