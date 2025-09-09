using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardMinLengthAttributeTests
{
    private sealed class TestModel
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void ReturnsSuccess_WhenStringIsLongerThanMinLength()
    {
        var model = new TestModel { Name = "abcdef" };
        var attr = new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var result = attr.GetValidationResult(model.Name, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenStringIsEqualToMinLength()
    {
        var model = new TestModel { Name = "abcde" };
        var attr = new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var result = attr.GetValidationResult(model.Name, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenStringIsShorterThanMinLength()
    {
        var model = new TestModel { Name = "abc" };
        var attr = new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var result = attr.GetValidationResult(model.Name, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var model = new TestModel { Name = null };
        var attr = new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var result = attr.GetValidationResult(model.Name, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSetToTurkish()
    {
        var model = new TestModel { Name = "abc" };
        var attr = new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var previousCulture = System.Globalization.CultureInfo.CurrentUICulture;
        try
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("tr");
            var result = attr.GetValidationResult(model.Name, ctx);
            Assert.NotNull(result);
            Assert.Contains("Kullanıcı adı", result.ErrorMessage);
            Assert.Contains("en az", result.ErrorMessage);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentUICulture = previousCulture;
        }
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardMinLengthAttribute(5, null!, "Username_MinLength"));
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardMinLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), null!));
    }
}

