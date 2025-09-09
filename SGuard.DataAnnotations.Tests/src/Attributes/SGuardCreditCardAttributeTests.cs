using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardCreditCardAttributeTests
{
    private sealed class TestModel
    {
        public string? CreditCardNumber { get; set; }
        public string? SecondaryCard { get; set; }
        public object? ObjectValue { get; set; }
        public int IntValue { get; set; }
    }

    #region Basic Functionality Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidVisa()
    {
        var model = new TestModel { CreditCardNumber = "4111111111111111" }; // Valid Visa test number
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidMasterCard()
    {
        var model = new TestModel { CreditCardNumber = "5555555555554444" }; // Valid MasterCard test number
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidAmex()
    {
        var model = new TestModel { CreditCardNumber = "378282246310005" }; // Valid Amex test number
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidDiscover()
    {
        var model = new TestModel { CreditCardNumber = "6011111111111117" }; // Valid Discover test number
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsInvalidCreditCard()
    {
        var model = new TestModel { CreditCardNumber = "1234567890123456" }; // Invalid credit card number
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueContainsLetters()
    {
        var model = new TestModel { CreditCardNumber = "411111111111111a" }; // Contains letter
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsTooShort()
    {
        var model = new TestModel { CreditCardNumber = "411111111" }; // Too short
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsTooLong()
    {
        var model = new TestModel { CreditCardNumber = "41111111111111111111" }; // Too long
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Null and Empty Value Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsNull()
    {
        var model = new TestModel { CreditCardNumber = null };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsEmpty()
    {
        var model = new TestModel { CreditCardNumber = "" };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsWhitespace()
    {
        var model = new TestModel { CreditCardNumber = "   " };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Credit Card Format Tests

    [Fact]
    public void ReturnsSuccess_WhenValueHasSpaces()
    {
        var model = new TestModel { CreditCardNumber = "4111 1111 1111 1111" }; // Valid Visa with spaces
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueHasDashes()
    {
        var model = new TestModel { CreditCardNumber = "4111-1111-1111-1111" }; // Valid Visa with dashes
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueHasSpecialCharacters()
    {
        var model = new TestModel { CreditCardNumber = "4111*1111*1111*1111" }; // Invalid characters
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Different Credit Card Types Tests

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidVisaElectron()
    {
        var model = new TestModel { CreditCardNumber = "4917300800000000" }; // Valid Visa Electron
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidDinersClub()
    {
        var model = new TestModel { CreditCardNumber = "30569309025904" }; // Valid Diners Club
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenValueIsValidJCB()
    {
        var model = new TestModel { CreditCardNumber = "3530111333300000" }; // Valid JCB
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region Luhn Algorithm Tests

    [Fact]
    public void ReturnsError_WhenLuhnChecksumFails()
    {
        var model = new TestModel { CreditCardNumber = "4111111111111112" }; // Invalid checksum (should be 1111)
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsSuccess_WhenNumberIsAllZeros()
    {
        var model = new TestModel { CreditCardNumber = "0000000000000000" }; // All zeros - CreditCardAttribute considers this valid
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenNumberIsAllSameDigit()
    {
        var model = new TestModel { CreditCardNumber = "1111111111111111" }; // All same digits
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Data Type Tests

    [Fact]
    public void ReturnsError_WhenValueIsNotString()
    {
        var model = new TestModel { IntValue = 123456789 };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.IntValue) };
        
        var result = attr.GetValidationResult(model.IntValue, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenValueIsObject()
    {
        var model = new TestModel { ObjectValue = new object() };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
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
            new SGuardCreditCardAttribute(null!, "Email_InvalidFormat"));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenResourceNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), null!));
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        
        Assert.Equal(typeof(Resources.SGuardDataAnnotations), attr.ErrorMessageResourceType);
        Assert.Equal("Email_InvalidFormat", attr.ErrorMessageResourceName);
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void PaymentForm_ValidCreditCard_Success()
    {
        var model = new TestModel { CreditCardNumber = "4242424242424242" }; // Valid test Visa
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber), DisplayName = "Credit Card Number" };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void PaymentForm_InvalidCreditCard_Failure()
    {
        var model = new TestModel { CreditCardNumber = "1234-5678-9012-3456" }; // Invalid card
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber), DisplayName = "Credit Card Number" };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void EcommerceCheckout_FormattedCard_Success()
    {
        var model = new TestModel { CreditCardNumber = "5555 5555 5555 4444" }; // Valid formatted MasterCard
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber), DisplayName = "Card Number" };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
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
            
            var model = new TestModel { CreditCardNumber = "invalid-card" };
            var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber), DisplayName = "Credit Card" };
            
            var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
            
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
            var model = new TestModel { CreditCardNumber = "invalid-card" };
            var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
            var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber), DisplayName = "Credit Card" };
            
            var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
            
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
    public void HandlesLeadingTrailingWhitespace()
    {
        var model = new TestModel { CreditCardNumber = "  4111111111111111  " }; // Valid card with whitespace
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenTabsAndNewlines()
    {
        var model = new TestModel { CreditCardNumber = "4111\t1111\n1111\r1111" }; // Card with tabs/newlines - CreditCardAttribute treats this as invalid
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesUnicodeDigits()
    {
        var model = new TestModel { CreditCardNumber = "４１１１１１１１１１１１１１１１" }; // Full-width digits
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        // Unicode digits should be invalid
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void HandlesMixedValidInvalidCharacters()
    {
        var model = new TestModel { CreditCardNumber = "4111 1111 1111 111X" }; // Valid format but invalid char at end
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_WhenOnlyWhitespace()
    {
        var model = new TestModel { CreditCardNumber = " \t \n \r " }; // Only whitespace - CreditCardAttribute treats this as invalid
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Test Credit Card Numbers

    [Theory]
    [InlineData("4000000000000002")] // Visa
    [InlineData("4000000000000010")] // Visa (debit)
    [InlineData("4000000000000028")] // Visa (prepaid)
    [InlineData("5555555555554444")] // MasterCard
    [InlineData("5105105105105100")] // MasterCard
    [InlineData("378282246310005")]  // American Express
    [InlineData("371449635398431")]  // American Express
    [InlineData("6011111111111117")] // Discover
    [InlineData("6011000990139424")] // Discover
    [InlineData("30569309025904")]   // Diners Club
    [InlineData("38520000023237")]   // Diners Club
    [InlineData("3530111333300000")] // JCB
    [InlineData("3566002020360505")] // JCB
    public void ReturnsSuccess_WhenValueIsValidTestCreditCard(string cardNumber)
    {
        var model = new TestModel { CreditCardNumber = cardNumber };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result);
    }

    [Theory]
    [InlineData("4000000000000001")] // Invalid Visa (bad checksum)
    [InlineData("5555555555554443")] // Invalid MasterCard (bad checksum)
    [InlineData("378282246310004")]  // Invalid Amex (bad checksum)
    [InlineData("6011111111111116")] // Invalid Discover (bad checksum)
    [InlineData("1234567890123456")] // Random invalid number
    [InlineData("9999999999999999")] // All nines
    public void ReturnsError_WhenValueIsInvalidTestCreditCard(string cardNumber)
    {
        var model = new TestModel { CreditCardNumber = cardNumber };
        var attr = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result = attr.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.NotEqual(ValidationResult.Success, result);
    }

    #endregion

    #region Multiple Attributes Tests

    [Fact]
    public void SupportsMultipleAttributes_OnSameProperty()
    {
        // This test verifies that multiple SGuardCreditCardAttribute can be applied to the same property
        var model = new TestModel { CreditCardNumber = "4111111111111111" };
        var attr1 = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var attr2 = new SGuardCreditCardAttribute(typeof(Resources.SGuardDataAnnotations), "Username_Required");
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.CreditCardNumber) };
        
        var result1 = attr1.GetValidationResult(model.CreditCardNumber, ctx);
        var result2 = attr2.GetValidationResult(model.CreditCardNumber, ctx);
        
        Assert.Equal(ValidationResult.Success, result1);
        Assert.Equal(ValidationResult.Success, result2);
    }

    #endregion
}
