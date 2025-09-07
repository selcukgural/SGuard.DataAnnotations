namespace SGuard.DataAnnotations;

/// <summary>
/// Validates that the decorated property's value is less than the value of another property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class SGuardLessThanAttribute : CompareToAttribute
{
    /// <summary>
    /// Creates a new instance of <see cref="SGuardLessThanAttribute"/>.
    /// </summary>
    /// <param name="otherProperty">The name of the property to compare with.</param>
    /// <param name="resourceType">The resource type for the error message.</param>
    /// <param name="resourceName">The resource name for the error message.</param>
    public SGuardLessThanAttribute(string otherProperty, Type resourceType, string resourceName) : base(
        otherProperty, ComparisonType.LessThan, resourceType, resourceName) { }
}