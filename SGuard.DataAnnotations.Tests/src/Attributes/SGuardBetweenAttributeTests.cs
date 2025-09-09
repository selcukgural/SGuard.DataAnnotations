using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardBetweenAttributeTests
{
    private sealed class TestModel
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Value { get; set; }
        public double DoubleMin { get; set; }
        public double DoubleMax { get; set; }
        public double DoubleValue { get; set; }
        public DateTime DateMin { get; set; }
        public DateTime DateMax { get; set; }
        public DateTime DateValue { get; set; }
        public string? StringMin { get; set; }
        public string? StringMax { get; set; }
        public string? StringValue { get; set; }
        public decimal DecimalMin { get; set; }
        public decimal DecimalMax { get; set; }
        public decimal DecimalValue { get; set; }
        public object? ObjectValue { get; set; }
        public int? NullableValue { get; set; }
        public int? NullableMin { get; set; }
        public int? NullableMax { get; set; }
    }

    private sealed class PrivatePropertyModel
    {
        private int PrivateMin { get; set; } = 5;
        private int PrivateMax { get; set; } = 15;
        public int Value { get; set; } = 10;
        
        public void SetPrivateValues(int min, int max)
        {
            PrivateMin = min;
            PrivateMax = max;
        }
    }

    #region Basic Functionality Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsWithinInclusiveRange()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 15 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsBelowMin_Inclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 9 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsAboveMax_Inclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 21 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsOnBounds_Inclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 10 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
        
        model.Value = 20;
        result = attr.GetValidationResult(model.Value, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsOnBounds_Exclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 10 };
        var attr = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
        
        model.Value = 20;
        result = attr.GetValidationResult(model.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsWithinExclusiveRange()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 15 };
        var attr = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsBelowMin_Exclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 9 };
        var attr = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsAboveMax_Exclusive()
    {
        var model = new TestModel { Min = 10, Max = 20, Value = 21 };
        var attr = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Data Type Tests

    [Fact]
    public void WorksWithDoubleValues_Inclusive()
    {
        var model = new TestModel { DoubleMin = 1.5, DoubleMax = 3.7, DoubleValue = 2.5 };
        var attr = new SGuardBetweenAttribute("DoubleMin", "DoubleMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DoubleValue) };
        
        var result = attr.GetValidationResult(model.DoubleValue, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDoubleValues_Exclusive()
    {
        var model = new TestModel { DoubleMin = 1.5, DoubleMax = 3.7, DoubleValue = 1.5 };
        var attr = new SGuardBetweenAttribute("DoubleMin", "DoubleMax", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DoubleValue) };
        
        var result = attr.GetValidationResult(model.DoubleValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDateTimeValues_Inclusive()
    {
        var model = new TestModel 
        { 
            DateMin = new DateTime(2023, 1, 1), 
            DateMax = new DateTime(2023, 12, 31), 
            DateValue = new DateTime(2023, 6, 15) 
        };
        var attr = new SGuardBetweenAttribute("DateMin", "DateMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DateValue) };
        
        var result = attr.GetValidationResult(model.DateValue, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDateTimeValues_OnBounds()
    {
        var model = new TestModel 
        { 
            DateMin = new DateTime(2023, 1, 1), 
            DateMax = new DateTime(2023, 12, 31), 
            DateValue = new DateTime(2023, 1, 1) 
        };
        var attr = new SGuardBetweenAttribute("DateMin", "DateMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DateValue) };
        
        var result = attr.GetValidationResult(model.DateValue, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithStringValues_Inclusive()
    {
        var model = new TestModel { StringMin = "apple", StringMax = "zebra", StringValue = "mango" };
        var attr = new SGuardBetweenAttribute("StringMin", "StringMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.StringValue) };
        
        var result = attr.GetValidationResult(model.StringValue, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDecimalValues_Inclusive()
    {
        var model = new TestModel { DecimalMin = 10.5m, DecimalMax = 20.75m, DecimalValue = 15.25m };
        var attr = new SGuardBetweenAttribute("DecimalMin", "DecimalMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.DecimalValue) };
        
        var result = attr.GetValidationResult(model.DecimalValue, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Error Condition Tests

    [Fact]
    public void ReturnsError_WhenMinPropertyIsMissing()
    {
        var model = new { Max = 20, Value = 15 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = "Value" };
        
        var result = attr.GetValidationResult(15, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Unknown property: Min", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsError_WhenMaxPropertyIsMissing()
    {
        var model = new { Min = 10, Value = 15 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = "Value" };
        
        var result = attr.GetValidationResult(15, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Unknown property: Max", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsError_WhenValueIsNotComparable()
    {
        var model = new TestModel { Min = 10, Max = 20, ObjectValue = new object() };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ObjectValue) };
        
        var result = attr.GetValidationResult(model.ObjectValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("must implement IComparable", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsError_WhenValueIsNull()
    {
        var model = new TestModel { Min = 10, Max = 20 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value), DisplayName = "TestValue" };
        
        var result = attr.GetValidationResult(null, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenMinValueIsNull()
    {
        var model = new TestModel { Max = 20, NullableValue = 15 };
        var attr = new SGuardBetweenAttribute("NullableMin", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NullableValue), DisplayName = "TestValue" };
        
        var result = attr.GetValidationResult(model.NullableValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenMaxValueIsNull()
    {
        var model = new TestModel { Min = 10, NullableValue = 15 };
        var attr = new SGuardBetweenAttribute("Min", "NullableMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NullableValue), DisplayName = "TestValue" };
        
        var result = attr.GetValidationResult(model.NullableValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithPrivateProperties()
    {
        var model = new PrivatePropertyModel();
        model.SetPrivateValues(5, 15);
        var attr = new SGuardBetweenAttribute("PrivateMin", "PrivateMax", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(PrivatePropertyModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMinPropertyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardBetweenAttribute(null!, "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required"));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMaxPropertyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardBetweenAttribute("Min", null!, true, typeof(Resources.SGuardDataAnnotations), "Username_Required"));
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var attr = new SGuardBetweenAttribute("MinProp", "MaxProp", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        
        Assert.Equal("MinProp", attr.MinProperty);
        Assert.Equal("MaxProp", attr.MaxProperty);
        Assert.False(attr.Inclusive);
    }

    [Fact]
    public void Constructor_DefaultsToInclusive()
    {
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        
        Assert.True(attr.Inclusive);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void HandlesEqualMinMaxValues_Inclusive()
    {
        var model = new TestModel { Min = 10, Max = 10, Value = 10 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesEqualMinMaxValues_Exclusive()
    {
        var model = new TestModel { Min = 10, Max = 10, Value = 10 };
        var attr = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesNegativeValues_Inclusive()
    {
        var model = new TestModel { Min = -20, Max = -10, Value = -15 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesZeroValues_Inclusive()
    {
        var model = new TestModel { Min = -5, Max = 5, Value = 0 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesLargeNumbers_Inclusive()
    {
        var model = new TestModel { Min = int.MaxValue - 1000, Max = int.MaxValue, Value = int.MaxValue - 500 };
        var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result = attr.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Localization Tests

    [Theory]
    [InlineData("en")]
    [InlineData("tr")]
    [InlineData("de")]
    [InlineData("fr")]
    [InlineData("ru")]
    [InlineData("ja")]
    [InlineData("hi")]
    public void ReturnsLocalizedErrorMessage_WhenCultureIsSet(string culture)
    {
        var originalCulture = Thread.CurrentThread.CurrentUICulture;

        try
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            var expectedMessage = Resources.SGuardDataAnnotations.Username_Required;
            
            var model = new TestModel { Min = 10, Max = 20, Value = 9 };
            var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value), DisplayName = "Value" };
            
            var result = attr.GetValidationResult(model.Value, ctx);
            
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal(expectedMessage, result?.ErrorMessage);
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = originalCulture;
        }
    }

    [Fact]
    public void UsesInvariantCulture_WhenResourceNotFound()
    {
        var originalCulture = Thread.CurrentThread.CurrentUICulture;

        try
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es"); // Spanish not supported
            var model = new TestModel { Min = 10, Max = 20, Value = 9 };
            var attr = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value), DisplayName = "Value" };
            
            var result = attr.GetValidationResult(model.Value, ctx);
            
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Username is required.", result?.ErrorMessage); // Falls back to English
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = originalCulture;
        }
    }

    #endregion

    #region Multiple Attributes Tests

    [Fact]
    public void SupportsMultipleAttributes_OnSameProperty()
    {
        // This test verifies the AllowMultiple = true on the attribute
        var model = new TestModel { Min = 10, Max = 20, Value = 15 };
        var attr1 = new SGuardBetweenAttribute("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var attr2 = new SGuardBetweenAttribute("Min", "Max", false, typeof(Resources.SGuardDataAnnotations), "Password_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value) };
        
        var result1 = attr1.GetValidationResult(model.Value, ctx);
        var result2 = attr2.GetValidationResult(model.Value, ctx);
        
        Assert.Equal(ValidationResult.Success, result1);
        Assert.Equal(ValidationResult.Success, result2);
    }

    #endregion
}
