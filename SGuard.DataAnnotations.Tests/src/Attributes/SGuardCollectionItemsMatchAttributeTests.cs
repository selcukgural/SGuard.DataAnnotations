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

    [Fact]
    public void AggregateAllErrors_StopsAtMaxErrors()
    {
        var items = Enumerable.Range(0, 1000).Select(i => "invalid" + i).ToList();
        var attr = new SGuardCollectionItemsMatchAttribute(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat")
        {
            AggregateAllErrors = true,
            MaxErrors = 3
        };
        var ctx = new ValidationContext(new object()) { MemberName = "Emails" };

        var result = attr.GetValidationResult(items, ctx);

        Assert.Equal(3, result?.ErrorMessage?.Split(';').Length);
        Assert.Contains("item #3", result?.ErrorMessage);
        Assert.DoesNotContain("item #4", result?.ErrorMessage);
    }

    [Fact]
    public void AggregateAllErrors_StopsEnumerating_AtMaxErrors()
    {
        var enumerated = 0;
        IEnumerable<string> Items()
        {
            while (true)
            {
                enumerated++;
                yield return "invalid";
            }
            // ReSharper disable once IteratorNeverReturns
        }

        var attr = new SGuardCollectionItemsMatchAttribute(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat")
        {
            AggregateAllErrors = true
        };
        var ctx = new ValidationContext(new object()) { MemberName = "Emails" };

        var result = attr.GetValidationResult(Items(), ctx);

        Assert.Equal(10, attr.MaxErrors);
        Assert.Equal(10, result?.ErrorMessage?.Split(';').Length);
        Assert.Equal(10, enumerated);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MaxErrors_BelowOne_Throws(int maxErrors)
    {
        var attr = new SGuardCollectionItemsMatchAttribute(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");

        Assert.Throws<ArgumentOutOfRangeException>(() => attr.MaxErrors = maxErrors);
        Assert.Equal(10, attr.MaxErrors);
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(ValidationAttribute))]
    [InlineData(typeof(SGuardMinLengthAttribute))]
    public void Constructor_RejectsTypesItCantCreate(Type type)
    {
        // Before, these were skipped, so every item passed.
        Assert.Throws<ArgumentException>(() =>
            new SGuardCollectionItemsMatchAttribute(type, typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat"));
        Assert.Throws<ArgumentException>(() =>
            new SGuardCollectionItemsMatchAttribute(new[] { typeof(RequiredAttribute), type }, typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat"));
    }

    [Fact]
    public void Constructor_RejectsAnEmptyOrNullEntry()
    {
        Assert.Throws<ArgumentException>(() =>
            new SGuardCollectionItemsMatchAttribute(Array.Empty<Type>(), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat"));
        Assert.Throws<ArgumentException>(() =>
            new SGuardCollectionItemsMatchAttribute(new Type[] { null! }, typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat"));
    }

    [Fact]
    public void SeveralValidators_AreAllApplied()
    {
        var attr = new SGuardCollectionItemsMatchAttribute(new[] { typeof(RequiredAttribute), typeof(EmailAddressAttribute) },
                                                           typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat");
        var ctx = new ValidationContext(new object()) { MemberName = "Emails" };

        Assert.Equal(ValidationResult.Success, attr.GetValidationResult(new[] { "a@b.c" }, ctx));
        Assert.NotEqual(ValidationResult.Success, attr.GetValidationResult(new[] { "a@b.c", "" }, ctx));
        Assert.NotEqual(ValidationResult.Success, attr.GetValidationResult(new[] { "a@b.c", "invalid" }, ctx));
    }
}
