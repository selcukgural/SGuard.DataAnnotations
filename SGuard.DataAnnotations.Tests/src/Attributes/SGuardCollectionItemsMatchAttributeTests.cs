using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.Attributes;

public sealed class SGuardCollectionItemsMatchAttributeTests
{
    private sealed class TestModel
    {
        [SGuardCollectionItemsMatch(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat")]
        public List<string> Emails { get; set; } = new();

        [SGuardCollectionItemsMatch(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat", AggregateAllErrors = true)]
        public List<string> AllErrorsEmails { get; set; } = new();

        [SGuardCollectionItemsMatch(typeof(RequiredAttribute), typeof(Resources.SGuardDataAnnotations), "Username_Required")]
        public List<string?> RequiredStrings { get; set; } = new();
    }

    [Fact]
    public void ReturnsSuccess_WhenAllItemsValid()
    {
        var model = new TestModel { Emails = new() { "foo@example.com", "bar@domain.com" } };
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Emails) };
        var prop = typeof(TestModel).GetProperty(nameof(TestModel.Emails))!;
        var attr = (SGuardCollectionItemsMatchAttribute)prop.GetCustomAttributes(typeof(SGuardCollectionItemsMatchAttribute), false)[0];

        var result = attr.GetValidationResult(model.Emails, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ReturnsError_OnFirstInvalidItem_WhenAggregateAllErrorsFalse()
    {
        var model = new TestModel { Emails = new() { "foo@example.com", "invalid" } };
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Emails) };
        var prop = typeof(TestModel).GetProperty(nameof(TestModel.Emails))!;
        var attr = (SGuardCollectionItemsMatchAttribute)prop.GetCustomAttributes(typeof(SGuardCollectionItemsMatchAttribute), false)[0];

        var result = attr.GetValidationResult(model.Emails, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Email address is not in a valid format.", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsAllErrors_WhenAggregateAllErrorsTrue()
    {
        var model = new TestModel { AllErrorsEmails = new() { "foo@example.com", "invalid1", "invalid2" } };
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.AllErrorsEmails) };
        var prop = typeof(TestModel).GetProperty(nameof(TestModel.AllErrorsEmails))!;
        var attr = (SGuardCollectionItemsMatchAttribute)prop.GetCustomAttributes(typeof(SGuardCollectionItemsMatchAttribute), false)[0];
        attr.AggregateAllErrors = true;

        var result = attr.GetValidationResult(model.AllErrorsEmails, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("item #2", result?.ErrorMessage);
        Assert.Contains("item #3", result?.ErrorMessage);
        Assert.Contains("Email address is not in a valid format.", result?.ErrorMessage);
        Assert.Equal(2, result?.ErrorMessage?.Split(';').Length);
    }

    [Fact]
    public void ReturnsError_WhenItemNull_AndRequiredAttribute()
    {
        var model = new TestModel { RequiredStrings = new() { "foo", null, "" } };
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.RequiredStrings) };
        var prop = typeof(TestModel).GetProperty(nameof(TestModel.RequiredStrings))!;
        var attr = (SGuardCollectionItemsMatchAttribute)prop.GetCustomAttributes(typeof(SGuardCollectionItemsMatchAttribute), false)[0];

        var result = attr.GetValidationResult(model.RequiredStrings, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("Username is required.", result?.ErrorMessage);
    }

    [Fact]
    public void ReturnsSuccess_WhenCollectionIsNull()
    {
        var model = new TestModel { Emails = null! };
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Emails) };
        var prop = typeof(TestModel).GetProperty(nameof(TestModel.Emails))!;
        var attr = (SGuardCollectionItemsMatchAttribute)prop.GetCustomAttributes(typeof(SGuardCollectionItemsMatchAttribute), false)[0];

        var result = attr.GetValidationResult(model.Emails, ctx);
        Assert.Equal(ValidationResult.Success, result);
    }
}