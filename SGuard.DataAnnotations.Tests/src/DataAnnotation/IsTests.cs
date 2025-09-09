using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Tests.DataAnnotation;

public sealed class IsTests
{
    private sealed class TestModelValid
    {
        [Required] public string Name { get; set; } = "Valid";
    }

    private sealed class TestModelInvalid
    {
        [Required] public string Name { get; set; }
    }

    [Fact]
    public void DataAnnotationsValid_ValidObject_ReturnsTrue()
    {
        var model = new TestModelValid();
        var result = Is.DataAnnotationsValid(model);
        Assert.True(result);
    }

    [Fact]
    public void DataAnnotationsValid_InvalidObject_ReturnsFalse()
    {
        var model = new TestModelInvalid();
        var result = Is.DataAnnotationsValid(model);
        Assert.False(result);
    }

    [Fact]
    public void DataAnnotationsValid_CallbackReceivesSuccessOnTrue()
    {
        var model = new TestModelValid();
        GuardOutcome? observed = null;
        var result = Is.DataAnnotationsValid(model, callback: outcome => observed = outcome);
        Assert.True(result);
        Assert.Equal(GuardOutcome.Success, observed);
    }

    [Fact]
    public void DataAnnotationsValid_CallbackReceivesFailureOnFalse()
    {
        var model = new TestModelInvalid();
        GuardOutcome? observed = null;
        var result = Is.DataAnnotationsValid(model, callback: outcome => observed = outcome);
        Assert.False(result);
        Assert.Equal(GuardOutcome.Failure, observed);
    }

    [Fact]
    public void DataAnnotationsValid_NullCallback_DoesNotThrow()
    {
        var model = new TestModelValid();
        var ex = Record.Exception(() => Is.DataAnnotationsValid(model, callback: null));
        Assert.Null(ex);
    }
}