using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardRequiredIfAttributeTests
{
    private sealed class TestModel
    {
        public string? Dependent { get; set; }
        public string? Value { get; set; }
    }

    private static ValidationContext GetContext(TestModel model) => new(model);

    [Fact]
    public void ReturnsSuccess_WhenConditionNotMet()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var model = new TestModel { Dependent = "other", Value = null };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenConditionMet_AndValueIsNull()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var model = new TestModel { Dependent = "trigger", Value = null };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenConditionMet_AndValueIsEmptyString()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var model = new TestModel { Dependent = "trigger", Value = "" };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenConditionMet_AndValueIsNonEmptyString()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var model = new TestModel { Dependent = "trigger", Value = "valid" };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDependentPropertyDoesNotExist()
    {
        var attribute = new SGuardRequiredIfAttribute("NonExistent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var model = new TestModel { Dependent = "trigger", Value = "valid" };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Unknown property", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsSuccess_WhenInvertIsTrue_AndConditionIsMet_AndValueIsNull()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required", invert: true);
        var model = new TestModel { Dependent = "trigger", Value = null };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenInvertIsTrue_AndConditionIsNotMet_AndValueIsNull()
    {
        var attribute = new SGuardRequiredIfAttribute("Dependent", "trigger", typeof(Resources.SGuardDataAnnotations), "Email_Required", invert: true);
        var model = new TestModel { Dependent = "other", Value = null };
        var result = attribute.GetValidationResult(model.Value, GetContext(model));
        Assert.NotEqual(ValidationResult.Success, result);
    }
}