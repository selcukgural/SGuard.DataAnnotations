using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardPhoneAttributeTests
{
    private sealed class TestModel
    {
        public string? Phone { get; set; }
    }

    [Fact]
    public void ReturnsSuccess_WhenPhoneIsValid()
    {
        var model = new TestModel { Phone = "+1-800-555-1234" };
        var attr = new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Phone) };
        var result = attr.GetValidationResult(model.Phone, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenPhoneIsInvalid()
    {
        var model = new TestModel { Phone = "not-a-phone" };
        var attr = new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Phone) };
        var result = attr.GetValidationResult(model.Phone, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenPhoneIsNull()
    {
        var model = new TestModel { Phone = null };
        var attr = new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Phone) };
        var result = attr.GetValidationResult(model.Phone, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenPhoneIsEmptyString()
    {
        var model = new TestModel { Phone = "" };
        var attr = new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Phone) };
        var result = attr.GetValidationResult(model.Phone, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSetToTurkish()
    {
        var model = new TestModel { Phone = "not-a-phone" };
        var attr = new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Phone) };
        var previousCulture = System.Globalization.CultureInfo.CurrentUICulture;
        try
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("tr");
            var result = attr.GetValidationResult(model.Phone, ctx);
            Assert.NotNull(result);
            Assert.Contains("Telefon numarası", result.ErrorMessage);
            Assert.Contains("geçerli", result.ErrorMessage);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentUICulture = previousCulture;
        }
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardPhoneAttribute(null!, "Profile_Phone_Invalid"));
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardPhoneAttribute(typeof(Resources.SGuardDataAnnotations), null!));
    }
}
