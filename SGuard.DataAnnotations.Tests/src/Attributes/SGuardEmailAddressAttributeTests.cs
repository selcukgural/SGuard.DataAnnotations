using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardEmailAddressAttributeTests
{
    private sealed class TestModel
    {
        public string? Email { get; set; }
        public string? SecondaryEmail { get; set; }
        public object? ObjectValue { get; set; }
        public int IntValue { get; set; }
    }

    #region Basic Functionality Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidEmail()
    {
        var model = new TestModel { Email = "user@example.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsInvalidEmail()
    {
        var model = new TestModel { Email = "invalid-email" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsMissingAtSymbol()
    {
        var model = new TestModel { Email = "userexample.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueHasMultipleAtSymbols()
    {
        var model = new TestModel { Email = "user@@example.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsMissingLocalPart()
    {
        var model = new TestModel { Email = "@example.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsMissingDomainPart()
    {
        var model = new TestModel { Email = "user@" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Null and Empty Value Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var model = new TestModel { Email = null };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsEmpty()
    {
        var model = new TestModel { Email = "" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsWhitespace()
    {
        var model = new TestModel { Email = "   " };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Valid Email Format Tests

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.org")]
    [InlineData("user.name+tag@example.co.uk")]
    [InlineData("user_name@domain-name.com")]
    [InlineData("123@456.com")]
    [InlineData("a@b.co")]
    [InlineData("user@subdomain.example.com")]
    [InlineData("first.last@sub.domain.com")]
    [InlineData("user+label@example.com")]
    [InlineData("email@123.123.123.123")] // IP address format
    public void ReturnsSuccess_WhenValueIsValidEmailFormat(string email)
    {
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Invalid Email Format Tests

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("@missinglocalpart.com")]
    [InlineData("two@@domain.com")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    public void ReturnsError_WhenValueIsInvalidEmailFormat(string email)
    {
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region International Email Tests

    [Theory]
    [InlineData("用户@example.com")] // Chinese characters
    [InlineData("user@münchen.de")] // German umlaut
    [InlineData("müller@example.com")] // Umlaut in local part
    [InlineData("user@россия.рф")] // Russian domain
    [InlineData("test@日本.jp")] // Japanese domain
    public void ReturnsNull_WhenValueIsUnsupportedInternationalEmail(string email)
    {
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        var result = attr.GetValidationResult(model.Email, ctx);
        Assert.Null(result);
    }

    #endregion

    #region Data Type Tests

    [Fact]
    public void ReturnsError_WhenValueIsNotString()
    {
        var model = new TestModel { IntValue = 12345 };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.IntValue) };
        
        var result = attr.GetValidationResult(model.IntValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsObject()
    {
        var model = new TestModel { ObjectValue = new object() };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.ObjectValue) };
        
        var result = attr.GetValidationResult(model.ObjectValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenResourceTypeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardEmailAddressAttribute(null!, "Email_InvalidFormat"));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), null!));
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        
        Assert.Equal(typeof(Resources.SGuardDataAnnotations), attr.ErrorMessageResourceType);
        Assert.Equal("Email_InvalidFormat", attr.ErrorMessageResourceName);
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void UserRegistration_ValidEmail_Success()
    {
        var model = new TestModel { Email = "newuser@company.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Email Address" };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void UserRegistration_InvalidEmail_Failure()
    {
        var model = new TestModel { Email = "invalid.email" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Email Address" };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ContactForm_BusinessEmail_Success()
    {
        var model = new TestModel { Email = "contact@business-domain.co.uk" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Contact Email" };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void Newsletter_SubscriptionEmail_Success()
    {
        var model = new TestModel { Email = "subscriber.name+newsletter@example.org" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Subscription Email" };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
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
            var expectedMessage = Resources.SGuardDataAnnotations.Email_InvalidFormat;
            
            var model = new TestModel { Email = "invalid-email" };
            var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Email" };
            
            var result = attr.GetValidationResult(model.Email, ctx);
            
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
            var model = new TestModel { Email = "invalid-email" };
            var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email), DisplayName = "Email" };
            
            var result = attr.GetValidationResult(model.Email, ctx);
            
            Assert.NotEqual(ValidationResult.Success, result);
            // Should fall back to English
            Assert.NotNull(result?.ErrorMessage);
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = originalCulture;
        }
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void HandlesVeryLongLocalPart()
    {
        var longLocalPart = new string('a', 64); // RFC limit for local part
        var email = $"{longLocalPart}@example.com";
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesVeryLongDomainPart()
    {
        var longDomain = new string('a', 60); // Close to domain length limit
        var email = $"user@{longDomain}.com";
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesSpecialCharactersInLocalPart()
    {
        var model = new TestModel { Email = "user.name+tag@example.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesHyphenInDomainName()
    {
        var model = new TestModel { Email = "user@sub-domain.example-site.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesNumbersInDomainAndLocalPart()
    {
        var model = new TestModel { Email = "user123@domain456.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenEmailHasTrailingSpaces()
    {
        var model = new TestModel { Email = "user@example.com " };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        // EmailAddressAttribute actually accepts trailing spaces
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenEmailHasLeadingSpaces()
    {
        var model = new TestModel { Email = " user@example.com" };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        // EmailAddressAttribute actually accepts leading spaces
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Common Email Providers

    [Theory]
    [InlineData("user@gmail.com")]
    [InlineData("user@outlook.com")]
    [InlineData("user@yahoo.com")]
    [InlineData("user@hotmail.com")]
    [InlineData("user@aol.com")]
    [InlineData("user@icloud.com")]
    [InlineData("user@protonmail.com")]
    [InlineData("user@live.com")]
    [InlineData("user@msn.com")]
    [InlineData("user@yandex.com")]
    public void ReturnsSuccess_WhenValueIsCommonEmailProvider(string email)
    {
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Business Email Formats

    [Theory]
    [InlineData("john.doe@company.com")]
    [InlineData("j.smith@tech-startup.io")]
    [InlineData("admin@nonprofit.org")]
    [InlineData("support@service.net")]
    [InlineData("info@business.co.uk")]
    [InlineData("sales@enterprise.eu")]
    [InlineData("marketing@brand.ca")]
    [InlineData("hr@corporation.au")]
    public void ReturnsSuccess_WhenValueIsBusinessEmailFormat(string email)
    {
        var model = new TestModel { Email = email };
        var attr = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result = attr.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Multiple Attributes Tests

    [Fact]
    public void SupportsMultipleAttributes_OnSameProperty()
    {
        // This test verifies that multiple SGuardEmailAddressAttribute can be applied to the same property
        var model = new TestModel { Email = "user@example.com" };
        var attr1 = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var attr2 = new SGuardEmailAddressAttribute(typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Email) };
        
        var result1 = attr1.GetValidationResult(model.Email, ctx);
        var result2 = attr2.GetValidationResult(model.Email, ctx);
        
        Assert.Equal(ValidationResult.Success, result1);
        Assert.Equal(ValidationResult.Success, result2);
    }

    #endregion
}
