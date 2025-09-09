using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardRequiredCollectionAttributeTests
{
    [Fact]
    public void ReturnsError_WhenValueIsNull()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(null, new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenArrayIsEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(new int[0], new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenArrayIsNotEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(new[] { 1 }, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenListIsEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(new List<string>(), new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenListIsNotEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(new List<string> { "item" }, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDictionaryIsEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(new Dictionary<int, string>(), new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenDictionaryIsNotEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var dict = new Dictionary<int, string> { { 1, "value" } };
        var result = attribute.GetValidationResult(dict, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenIEnumerableIsEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        IEnumerable<int> enumerable = new List<int>();
        var result = attribute.GetValidationResult(enumerable, new ValidationContext(new object()));
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenIEnumerableIsNotEmpty()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        IEnumerable<int> enumerable = new List<int> { 42 };
        var result = attribute.GetValidationResult(enumerable, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsNotACollection()
    {
        var attribute = new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Email_Required");
        var result = attribute.GetValidationResult(123, new ValidationContext(new object()));
        Assert.Equal(ValidationResult.Success, result);
    }
}