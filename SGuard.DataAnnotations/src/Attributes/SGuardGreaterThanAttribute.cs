namespace SGuard.DataAnnotations;

using System;

/// <summary>
/// Validates that the decorated property's value is greater than the value of another property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class SGuardGreaterThanAttribute : CompareToAttribute
{
    /// <summary>
    /// Creates a new instance of <see cref="SGuardGreaterThanAttribute"/>.
    /// </summary>
    /// <param name="otherProperty">The name of the property to compare with.</param>
    /// <param name="resourceType">The resource type for the error message.</param>
    /// <param name="resourceName">The resource name for the error message.</param>
    public SGuardGreaterThanAttribute(string otherProperty, Type resourceType, string resourceName) : base(
        otherProperty, ComparisonType.GreaterThan, resourceType, resourceName) { }
}