using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardUrlAttributeTests
{
    [Fact]
    public void ReturnsSuccess_WhenValueIsValidUrl()
    {
        var attr = new SGuardUrlAttribute(typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var result = attr.GetValidationResult("https://example.com", new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsInvalidUrl()
    {
        var attr = new SGuardUrlAttribute(typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var result = attr.GetValidationResult("not a url", new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var attr = new SGuardUrlAttribute(typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var result = attr.GetValidationResult(null, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsWhitespaceString()
    {
        var attr = new SGuardUrlAttribute(typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var result = attr.GetValidationResult("   ", new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardUrlAttribute(null!, "General_UnexpectedError"));
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardUrlAttribute(typeof(Resources.SGuardDataAnnotations), null!));
    }
}

