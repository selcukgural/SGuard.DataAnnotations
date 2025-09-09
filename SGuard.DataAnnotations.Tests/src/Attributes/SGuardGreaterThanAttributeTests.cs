using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardGreaterThanAttributeTests
{
    private class TestModel
    {
        public int Value { get; set; }
        public int Other { get; set; }
        public decimal DecimalValue { get; set; }
        public decimal DecimalOther { get; set; }
        public DateTime DateValue { get; set; }
        public DateTime DateOther { get; set; }
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsGreaterThanOther()
    {
        var model = new TestModel { Value = 10, Other = 5 };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.Other), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsEqualToOther()
    {
        var model = new TestModel { Value = 5, Other = 5 };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.Other), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsLessThanOther()
    {
        var model = new TestModel { Value = 3, Other = 5 };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.Other), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenDecimalValueIsGreaterThanOther()
    {
        var model = new TestModel { DecimalValue = 10.5m, DecimalOther = 5.5m };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.DecimalOther), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DecimalValue) };
        var result = attr.GetValidationResult(model.DecimalValue, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDecimalValueIsNotGreaterThanOther()
    {
        var model = new TestModel { DecimalValue = 2.2m, DecimalOther = 2.2m };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.DecimalOther), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DecimalValue) };
        var result = attr.GetValidationResult(model.DecimalValue, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenDateValueIsGreaterThanOther()
    {
        var now = DateTime.Now;
        var model = new TestModel { DateValue = now.AddDays(1), DateOther = now };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.DateOther), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DateValue) };
        var result = attr.GetValidationResult(model.DateValue, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDateValueIsNotGreaterThanOther()
    {
        var now = DateTime.Now;
        var model = new TestModel { DateValue = now, DateOther = now.AddDays(1) };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.DateOther), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DateValue) };
        var result = attr.GetValidationResult(model.DateValue, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenOtherPropertyDoesNotExist()
    {
        var model = new TestModel { Value = 10 };
        var attr = new SGuardGreaterThanAttribute("NonExistent", typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsNull()
    {
        var model = new { Value = (int?)null, Other = 5 };
        var attr = new SGuardGreaterThanAttribute("Other", typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = "Value" };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenOtherIsNull()
    {
        var model = new { Value = 5, Other = (int?)null };
        var attr = new SGuardGreaterThanAttribute("Other", typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = "Value" };
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSetToTurkish()
    {
        var model = new TestModel { Value = 1, Other = 5 };
        var attr = new SGuardGreaterThanAttribute(nameof(TestModel.Other), typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
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
}
