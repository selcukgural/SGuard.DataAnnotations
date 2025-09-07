using System.ComponentModel.DataAnnotations;
using SGuard.DataAnnotations.Exceptions;

namespace SGuard.DataAnnotations.Tests.Guards.DataAnnotation
{
    public class ThrowIfTests
    {
        private class TestModelValid
        {
            [Required] public string Name { get; set; } = "Valid";
        }

        private class TestModelInvalid
        {
            [Required] public string Name { get; set; }
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

        [Fact]
        public void DataAnnotationsInValid_InvalidObject_ThrowsCustomExceptionWithArgs()
        {
            var model = new TestModelInvalid();
            Assert.Throws<ArgumentException>(() => ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new object[] { "Custom error" }));
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
                ThrowIf.DataAnnotationsInValid<ArgumentException>(model,exception: null, callback: _ => callbackInvoked = true));
            Assert.False(callbackInvoked);
        }
    }
}