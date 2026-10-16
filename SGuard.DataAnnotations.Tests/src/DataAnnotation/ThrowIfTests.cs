using System.ComponentModel.DataAnnotations;
using SGuard.DataAnnotations.Exceptions;

namespace SGuard.DataAnnotations.Tests.DataAnnotation;

public sealed class ThrowIfTests
{
    private sealed class TestModelValid
    {
        [Required] public string Name { get; set; } = "Valid";
    }

    private sealed class TestModelInvalid
    {
        [Required] public string? Name { get; set; }
    }

    [Fact]
    public void DataAnnotationsInValid_ValidObject_DoesNotThrow()
    {
        var model = new TestModelValid();
        var ex = Record.Exception(() => ThrowIf.DataAnnotationsInValid(model));
        Assert.Null(ex);
    }

    [Fact]
    public void DataAnnotationsInValid_InvalidObject_ThrowsDataAnnotationsException()
    {
        var model = new TestModelInvalid();
        Assert.Throws<DataAnnotationsException>(() => ThrowIf.DataAnnotationsInValid(model));
    }

    [Fact]
    public void DataAnnotationsInValid_InvalidObject_ThrowsCustomException()
    {
        var model = new TestModelInvalid();
        Assert.Throws<ArgumentException>(() => ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new ArgumentException("Custom error")));
    }

#pragma warning disable SGUARDDA001 // The constructor-arguments overload is obsolete but still supported.
    [Fact]
    public void DataAnnotationsInValid_InvalidObject_ThrowsCustomExceptionWithArgs()
    {
        var model = new TestModelInvalid();
        Assert.Throws<ArgumentException>(() => ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new object[] { "Custom error" }));
    }

    [Fact]
    public void DataAnnotationsInValid_NoMatchingConstructor_ThrowsInvalidOperationException()
    {
        var model = new TestModelInvalid();
        var ex = Assert.Throws<InvalidOperationException>(() =>
                                                              ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new object[] { 42, 42, 42 }));
        Assert.Contains("No matching constructor", ex.Message);
    }
#pragma warning restore SGUARDDA001

    [Fact]
    public void DataAnnotationsInValid_InvalidObject_ThrowsTheFactorysException()
    {
        var model = new TestModelInvalid();
        var ex = Assert.Throws<ArgumentException>(() =>
                                                      ThrowIf.DataAnnotationsInValid(model, () => new ArgumentException("Custom error")));
        Assert.Equal("Custom error", ex.Message);
    }

    [Fact]
    public void DataAnnotationsInValid_ValidObject_DoesNotCallTheFactory()
    {
        var model = new TestModelValid();
        var called = false;
        ThrowIf.DataAnnotationsInValid(model, () =>
        {
            called = true;
            return new ArgumentException();
        });
        Assert.False(called);
    }

    [Fact]
    public void DataAnnotationsInValid_FactoryReturnsNull_ThrowsInvalidOperationException()
    {
        var model = new TestModelInvalid();
        GuardOutcome? observed = null;
        Assert.Throws<InvalidOperationException>(() =>
                                                     ThrowIf.DataAnnotationsInValid<ArgumentException>(model, () => null!,
                                                         callback: outcome => observed = outcome));
        Assert.Equal(GuardOutcome.Failure, observed);
    }

    [Fact]
    public void DataAnnotationsInValid_NullFactory_ThrowsArgumentNullExceptionAndDoesNotInvokeCallback()
    {
        var model = new TestModelValid();
        var callbackInvoked = false;
        Assert.Throws<ArgumentNullException>(() =>
                                                 ThrowIf.DataAnnotationsInValid(model, exceptionFactory: (Func<ArgumentException>)null!,
                                                                                callback: _ => callbackInvoked = true));
        Assert.False(callbackInvoked);
    }

    [Fact]
    public void DataAnnotationsInValid_CallbackIsInvoked()
    {
        var model = new TestModelInvalid();
        bool callbackInvoked = false;
        Assert.Throws<DataAnnotationsException>(() =>
                                                    ThrowIf.DataAnnotationsInValid(model, callback: _ => callbackInvoked = true));
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void DataAnnotationsInValid_CallbackReceivesFailureOnThrow()
    {
        var model = new TestModelInvalid();
        GuardOutcome? observed = null;
        Assert.Throws<DataAnnotationsException>(() =>
                                                    ThrowIf.DataAnnotationsInValid(model, callback: outcome => observed = outcome));
        Assert.Equal(GuardOutcome.Failure, observed);
    }

    [Fact]
    public void DataAnnotationsInValid_CallbackReceivesSuccessOnPass()
    {
        var model = new TestModelValid();
        GuardOutcome? observed = null;
        var ex = Record.Exception(() =>
                                      ThrowIf.DataAnnotationsInValid(model, callback: outcome => observed = outcome));
        Assert.Null(ex);
        Assert.Equal(GuardOutcome.Success, observed);
    }

    [Fact]
    public void DataAnnotationsInValid_NullCallback_DoesNotThrow()
    {
        var model = new TestModelValid();
        var ex = Record.Exception(() => ThrowIf.DataAnnotationsInValid(model, callback: null));
        Assert.Null(ex);
    }

    [Fact]
    public void DataAnnotationsInValid_NullExceptionInstance_ThrowsArgumentNullExceptionAndDoesNotInvokeCallback()
    {
        var model = new TestModelInvalid();
        bool callbackInvoked = false;
        Assert.Throws<ArgumentNullException>(() =>
                                                 ThrowIf.DataAnnotationsInValid<ArgumentException>(model, exception: null!, callback: _ => callbackInvoked = true));
        Assert.False(callbackInvoked);
    }
}