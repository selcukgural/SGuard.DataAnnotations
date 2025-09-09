using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardCompareAttributeTests
{
    private sealed class TestModel
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public string? NonExistentProperty { get; set; }
        public object? ObjectValue1 { get; set; }
        public object? ObjectValue2 { get; set; }
        public int? NullableValue1 { get; set; }
        public int? NullableValue2 { get; set; }
    }

    private sealed class PrivatePropertyModel
    {
        private string? PrivateProperty { get; set; } = "test";
        public string? PublicProperty { get; set; } = "test";
        
        public void SetPrivateProperty(string? value)
        {
            PrivateProperty = value;
        }
    }

    #region Basic Functionality Tests

    [Fact]
    public void ReturnsSuccess_WhenPropertiesAreEqual()
    {
        var model = new TestModel { Password = "Test123!", ConfirmPassword = "Test123!" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenPropertiesAreDifferent()
    {
        var model = new TestModel { Password = "Test123!", ConfirmPassword = "Different123!" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenBothPropertiesAreNull()
    {
        var model = new TestModel { Password = null, ConfirmPassword = null };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenOnePropertyIsNull()
    {
        var model = new TestModel { Password = "Test123!", ConfirmPassword = null };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenOtherPropertyIsNull()
    {
        var model = new TestModel { Password = null, ConfirmPassword = "Test123!" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenEmptyStringsAreEqual()
    {
        var model = new TestModel { Password = "", ConfirmPassword = "" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenEmptyStringComparedToNull()
    {
        var model = new TestModel { Password = null, ConfirmPassword = "" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Data Type Tests

    [Fact]
    public void WorksWithIntegerValues()
    {
        var model = new TestModel { Value1 = 42, Value2 = 42 };
        var attr = new SGuardCompareAttribute("Value1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value2) };
        
        var result = attr.GetValidationResult(model.Value2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDateTimeValues()
    {
        var date = new DateTime(2023, 6, 15, 10, 30, 0);
        var model = new TestModel { Date1 = date, Date2 = date };
        var attr = new SGuardCompareAttribute("Date1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Date2) };
        
        var result = attr.GetValidationResult(model.Date2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithDecimalValues()
    {
        var model = new TestModel { Amount1 = 99.99m, Amount2 = 99.99m };
        var attr = new SGuardCompareAttribute("Amount1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Amount2) };
        
        var result = attr.GetValidationResult(model.Amount2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithNullableValues_BothHaveValues()
    {
        var model = new TestModel { NullableValue1 = 100, NullableValue2 = 100 };
        var attr = new SGuardCompareAttribute("NullableValue1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NullableValue2) };
        
        var result = attr.GetValidationResult(model.NullableValue2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void WorksWithNullableValues_BothAreNull()
    {
        var model = new TestModel { NullableValue1 = null, NullableValue2 = null };
        var attr = new SGuardCompareAttribute("NullableValue1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NullableValue2) };
        
        var result = attr.GetValidationResult(model.NullableValue2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void FailsWithNullableValues_OnlyOneIsNull()
    {
        var model = new TestModel { NullableValue1 = 100, NullableValue2 = null };
        var attr = new SGuardCompareAttribute("NullableValue1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.NullableValue2) };
        
        var result = attr.GetValidationResult(model.NullableValue2, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Case Sensitivity Tests

    [Fact]
    public void IsCaseSensitive_WithStrings()
    {
        var model = new TestModel { Password = "Test123!", ConfirmPassword = "test123!" };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesWhitespace_Differences()
    {
        var model = new TestModel { Password = "Test123!", ConfirmPassword = " Test123! " };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Error Condition Tests

    [Fact]
    public void ReturnsError_WhenOtherPropertyDoesNotExist()
    {
        var model = new TestModel { ConfirmPassword = "Test123!" };
        var attr = new SGuardCompareAttribute("NonExistentProperty", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
        // CompareAttribute returns a generic error message when property doesn't exist
        Assert.NotNull(result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsError_WhenPrivatePropertyCannotBeAccessed()
    {
        var model = new PrivatePropertyModel();
        model.SetPrivateProperty("test");
        var attr = new SGuardCompareAttribute("PrivateProperty", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(PrivatePropertyModel.PublicProperty) };
        
        var result = attr.GetValidationResult(model.PublicProperty, ctx);
        
        // CompareAttribute cannot access private properties, so this should fail
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Could not find a property named PrivateProperty", result?.ErrorMessage ?? "");
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenOtherPropertyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardCompareAttribute(null!, typeof(Resources.SGuardDataAnnotations), "Password_Mismatch"));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardCompareAttribute("Password", null!, "Password_Mismatch"));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), null!));
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var attr = new SGuardCompareAttribute("TestProperty", typeof(Resources.SGuardDataAnnotations), "Test_Message");
        
        Assert.Equal("TestProperty", attr.OtherProperty);
        Assert.Equal(typeof(Resources.SGuardDataAnnotations), attr.ErrorMessageResourceType);
        Assert.Equal("Test_Message", attr.ErrorMessageResourceName);
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void PasswordConfirmation_Success()
    {
        var model = new TestModel 
        { 
            Password = "MySecure@Password123", 
            ConfirmPassword = "MySecure@Password123" 
        };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void PasswordConfirmation_Failure()
    {
        var model = new TestModel 
        { 
            Password = "MySecure@Password123", 
            ConfirmPassword = "DifferentPassword456" 
        };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void EmailConfirmation_Success()
    {
        var model = new TestModel 
        { 
            Email = "user@example.com", 
            ConfirmEmail = "user@example.com" 
        };
        var attr = new SGuardCompareAttribute("Email", typeof(Resources.SGuardDataAnnotations), "Email_AlreadyExists");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmEmail) };
        
        var result = attr.GetValidationResult(model.ConfirmEmail, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void EmailConfirmation_Failure()
    {
        var model = new TestModel 
        { 
            Email = "user@example.com", 
            ConfirmEmail = "different@example.com" 
        };
        var attr = new SGuardCompareAttribute("Email", typeof(Resources.SGuardDataAnnotations), "Email_AlreadyExists");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmEmail) };
        
        var result = attr.GetValidationResult(model.ConfirmEmail, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
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
            var expectedMessage = Resources.SGuardDataAnnotations.Password_Mismatch;
            
            var model = new TestModel { Password = "Test123!", ConfirmPassword = "Different123!" };
            var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword), DisplayName = "Confirm Password" };
            
            var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
            
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
            var model = new TestModel { Password = "Test123!", ConfirmPassword = "Different123!" };
            var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword), DisplayName = "Confirm Password" };
            
            var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
            
            Assert.NotEqual(ValidationResult.Success, result);
            // Should fall back to English
            Assert.Contains("do not match", result?.ErrorMessage?.ToLower() ?? "");
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = originalCulture;
        }
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void HandlesVeryLongStrings()
    {
        var longString = new string('A', 10000);
        var model = new TestModel { Password = longString, ConfirmPassword = longString };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesSpecialCharacters()
    {
        var specialString = "!@#$%^&*()_+-=[]{}|;:,.<>?`~";
        var model = new TestModel { Password = specialString, ConfirmPassword = specialString };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesUnicodeCharacters()
    {
        var unicodeString = "こんにちは世界🌍🔒🎉";
        var model = new TestModel { Password = unicodeString, ConfirmPassword = unicodeString };
        var attr = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result = attr.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesMinMaxDateTimeValues()
    {
        var model = new TestModel { Date1 = DateTime.MinValue, Date2 = DateTime.MinValue };
        var attr = new SGuardCompareAttribute("Date1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Date2) };
        
        var result = attr.GetValidationResult(model.Date2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
        
        model.Date1 = DateTime.MaxValue;
        model.Date2 = DateTime.MaxValue;
        result = attr.GetValidationResult(model.Date2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesMinMaxIntValues()
    {
        var model = new TestModel { Value1 = int.MinValue, Value2 = int.MinValue };
        var attr = new SGuardCompareAttribute("Value1", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Value2) };
        
        var result = attr.GetValidationResult(model.Value2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
        
        model.Value1 = int.MaxValue;
        model.Value2 = int.MaxValue;
        result = attr.GetValidationResult(model.Value2, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Multiple Attributes Tests

    [Fact]
    public void SupportsMultipleAttributes_OnSameProperty()
    {
        // This test verifies the AllowMultiple = true behavior (inherited from CompareAttribute)
        var model = new TestModel { Password = "Test123!", ConfirmPassword = "Test123!" };
        var attr1 = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch");
        var attr2 = new SGuardCompareAttribute("Password", typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ConfirmPassword) };
        
        var result1 = attr1.GetValidationResult(model.ConfirmPassword, ctx);
        var result2 = attr2.GetValidationResult(model.ConfirmPassword, ctx);
        
        Assert.Equal(ValidationResult.Success, result1);
        Assert.Equal(ValidationResult.Success, result2);
    }

    #endregion
}
