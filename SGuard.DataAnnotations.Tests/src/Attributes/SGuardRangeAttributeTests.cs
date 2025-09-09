using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardRangeAttributeTests
{
    private sealed class TestModelInt
    {
        public int Value { get; set; }
    }
    private sealed class TestModelDouble
    {
        public double Value { get; set; }
    }

    [Fact]
    public void ReturnsSuccess_WhenIntValueIsWithinRange()
    {
        var model = new TestModelInt { Value = 5 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenIntValueIsBelowRange()
    {
        var model = new TestModelInt { Value = 0 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenIntValueIsAboveRange()
    {
        var model = new TestModelInt { Value = 11 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenDoubleValueIsWithinRange()
    {
        var model = new TestModelDouble { Value = 5.5 };
        var attr = new SGuardRangeAttribute(1.0, 10.0, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelDouble.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDoubleValueIsBelowRange()
    {
        var model = new TestModelDouble { Value = 0.9 };
        var attr = new SGuardRangeAttribute(1.0, 10.0, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelDouble.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDoubleValueIsAboveRange()
    {
        var model = new TestModelDouble { Value = 10.1 };
        var attr = new SGuardRangeAttribute(1.0, 10.0, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelDouble.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsEqualToMinimum()
    {
        var model = new TestModelInt { Value = 1 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsEqualToMaximum()
    {
        var model = new TestModelInt { Value = 10 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(new TestModelInt()) { MemberName = nameof(TestModelInt.Value) };
        var result = attr.GetValidationResult(null, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSetToTurkish()
    {
        var model = new TestModelInt { Value = 0 };
        var attr = new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModelInt.Value) };
        var previousCulture = System.Globalization.CultureInfo.CurrentUICulture;
        try
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("tr");
            var result = attr.GetValidationResult(model.Value, ctx);
            Assert.NotNull(result);
            Assert.Equal(Resources.SGuardDataAnnotations.General_UnexpectedError, result.ErrorMessage);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentUICulture = previousCulture;
        }
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardRangeAttribute(1, 10, null!, "General_UnexpectedError"));
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardRangeAttribute(1, 10, typeof(Resources.SGuardDataAnnotations), null!));
    }
}

