using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardValidationAttributeBaseFallbackTests
{
    private sealed class DummyTestAttribute(Type resourceType, string resourceName) : SGuardValidationAttributeBase(resourceType, resourceName)
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
            new(FormatErrorMessage(validationContext.DisplayName ?? "Field"));
    }

    [Fact]
    public void Returns_MainResource_WhenResourceExists()
    {
        var attr = new DummyTestAttribute(typeof(Resources.SGuardDataAnnotations), nameof(Resources.SGuardDataAnnotations.Address_City_Required));
        var result = attr.FormatErrorMessage("TestField");
        Assert.Equal("City is required.", result);
    }

    [Fact]
    public void Returns_FallbackResource_IfMainResourceMissing_AndFallbackResourceExists()
    {
        var attr = new DummyTestAttribute(typeof(Resources.SGuardDataAnnotations), "NonExistingKey")
        {
            FallbackResourceName = nameof(Resources.SGuardDataAnnotations.Terms_NotAccepted)
        };
        var result = attr.FormatErrorMessage("TestField");
        Assert.Equal("You must accept the terms and conditions.", result);
    }

    [Fact]
    public void Returns_FallbackMessage_IfBothResourcesMissing_AndFallbackMessageSet()
    {
        var attr = new DummyTestAttribute(typeof(Resources.SGuardDataAnnotations), "[NonExisting]")
        {
            FallbackResourceName = "NonExisting",
            FallbackMessage = "Custom fallback message"
        };
        var result = attr.FormatErrorMessage("TestField");
        Assert.Equal("Custom fallback message", result);
    }

    [Fact]
    public void Returns_ResourceNameInBrackets_IfNoFallbacksExist()
    {
        var attr = new DummyTestAttribute(typeof(Resources.SGuardDataAnnotations), "NotExistResource");
        var result = attr.FormatErrorMessage("TestField");
        Assert.Equal("[NotExistResource]", result);
    }
}