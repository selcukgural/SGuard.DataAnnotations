using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardMaxCountAttributeTests
{
    private sealed class TestModel
    {
        public int[] IntArray { get; set; } = Array.Empty<int>();
        public List<string> StringList { get; set; } = new();
        public Dictionary<string, int> Dict { get; set; } = new();
        public ICollection<double> DoubleCollection { get; set; } = new List<double>();
        public IEnumerable<int> Enumerable { get; set; } = Array.Empty<int>();
        public string NotACollection { get; set; } = "abc";
    }

    [Fact]
    public void ReturnsSuccess_WhenArrayCountIsLessThanOrEqualToMax()
    {
        var model = new TestModel { IntArray = new[] { 1, 2, 3 } };
        var attr = new SGuardMaxCountAttribute(3, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.IntArray) };
        var result = attr.GetValidationResult(model.IntArray, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenArrayCountExceedsMax()
    {
        var model = new TestModel { IntArray = new[] { 1, 2, 3, 4 } };
        var attr = new SGuardMaxCountAttribute(3, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.IntArray) };
        var result = attr.GetValidationResult(model.IntArray, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenListCountIsEqualToMax()
    {
        var model = new TestModel { StringList = new List<string> { "a", "b" } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.StringList) };
        var result = attr.GetValidationResult(model.StringList, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenListCountExceedsMax()
    {
        var model = new TestModel { StringList = new List<string> { "a", "b", "c" } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.StringList) };
        var result = attr.GetValidationResult(model.StringList, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenDictionaryCountIsLessThanMax()
    {
        var model = new TestModel { Dict = new Dictionary<string, int> { { "a", 1 } } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Dict) };
        var result = attr.GetValidationResult(model.Dict, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenDictionaryCountExceedsMax()
    {
        var model = new TestModel { Dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Dict) };
        var result = attr.GetValidationResult(model.Dict, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenCollectionCountIsZero()
    {
        var model = new TestModel { DoubleCollection = new List<double>() };
        var attr = new SGuardMaxCountAttribute(0, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DoubleCollection) };
        var result = attr.GetValidationResult(model.DoubleCollection, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenCollectionCountIsGreaterThanZero()
    {
        var model = new TestModel { DoubleCollection = new List<double> { 1.1 } };
        var attr = new SGuardMaxCountAttribute(0, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DoubleCollection) };
        var result = attr.GetValidationResult(model.DoubleCollection, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenEnumerableCountIsLessThanOrEqualToMax()
    {
        var model = new TestModel { Enumerable = new[] { 1, 2 } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Enumerable) };
        var result = attr.GetValidationResult(model.Enumerable, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenEnumerableCountExceedsMax()
    {
        var model = new TestModel { Enumerable = new[] { 1, 2, 3 } };
        var attr = new SGuardMaxCountAttribute(2, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Enumerable) };
        var result = attr.GetValidationResult(model.Enumerable, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var attr = new SGuardMaxCountAttribute(1, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(new TestModel()) { MemberName = nameof(TestModel.IntArray) };
        var result = attr.GetValidationResult(null, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsNotACollection()
    {
        var model = new TestModel { NotACollection = "abc" };
        var attr = new SGuardMaxCountAttribute(1, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NotACollection) };
        var result = attr.GetValidationResult(model.NotACollection, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ThrowsArgumentOutOfRangeException_WhenMaxCountIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SGuardMaxCountAttribute(-1, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError"));
    }

    [Fact]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSetToTurkish()
    {
        var model = new TestModel { IntArray = new[] { 1, 2, 3, 4 } };
        var attr = new SGuardMaxCountAttribute(3, typeof(Resources.SGuardDataAnnotations), "General_UnexpectedError");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.IntArray) };
        var previousCulture = System.Globalization.CultureInfo.CurrentUICulture;
        try
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("tr");
            var result = attr.GetValidationResult(model.IntArray, ctx);
            Assert.NotNull(result);
            Assert.Equal(Resources.SGuardDataAnnotations.General_UnexpectedError, result.ErrorMessage);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentUICulture = previousCulture;
        }
    }
}

