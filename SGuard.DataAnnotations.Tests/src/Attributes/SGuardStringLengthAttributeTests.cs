using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardStringLengthAttributeTests
{
    [Fact]
    public void ReturnsSuccess_WhenStringLengthIsWithinMaximum()
    {
        var attr = new SGuardStringLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required");
        var result = attr.GetValidationResult("abc", new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenStringLengthExceedsMaximum()
    {
        var attr = new SGuardStringLengthAttribute(3, typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required");
        var result = attr.GetValidationResult("abcd", new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenStringIsNull()
    {
        var attr = new SGuardStringLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required");
        var result = attr.GetValidationResult(null, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenStringIsEmpty()
    {
        var attr = new SGuardStringLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required");
        var result = attr.GetValidationResult("", new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardStringLengthAttribute(5, null!, "Common_Collection_Required"));
    }

    [Fact]
    public void ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SGuardStringLengthAttribute(5, typeof(Resources.SGuardDataAnnotations), null!));
    }
}

