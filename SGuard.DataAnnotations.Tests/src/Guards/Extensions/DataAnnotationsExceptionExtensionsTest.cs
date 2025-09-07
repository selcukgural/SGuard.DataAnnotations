using System.ComponentModel.DataAnnotations;
using SGuard.DataAnnotations.Exceptions;
using SGuard.DataAnnotations.Extensions;
using SGuard.DataAnnotations.Tests.Guards.Exceptions;

namespace SGuard.DataAnnotations.Tests.Guards.Extensions;

public  sealed class DataAnnotationsExceptionExtensionsTest
{
[Fact]
    public void TryGetValidationErrors_ReturnsTrueAndPopulatesErrors_WhenValidationErrorsExist()
    {
        var validationErrors = new List<ValidationError>
        {
            new("Error 1", new[] { "Property1" }),
            new("Error 2", new[] { "Property2" })
        };
    
        var exception = new DataAnnotationsException(string.Empty, new List<ValidationResult>())
        {
            Data =
            {
                [DataAnnotationsExceptionTests.DataKey] = validationErrors
            }
        };

        var result = exception.TryGetValidationErrors(out var errors);
    
        Assert.True(result);
        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.Equal("Error 1", errors.First().Message);
        Assert.Contains("Property1", errors.First().Members);
    }
    
    [Fact]
    public void TryGetValidationErrors_ReturnsFalse_WhenNoValidationErrorsExist()
    {
        var exception = new DataAnnotationsException(string.Empty, new List<ValidationResult>());
    
        var result = exception.TryGetValidationErrors(out var errors);
    
        Assert.False(result);
        Assert.Empty(errors);
    }
    
}