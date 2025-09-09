using System.ComponentModel.DataAnnotations;
using SGuard.DataAnnotations.Exceptions;

namespace SGuard.DataAnnotations.Tests.Exceptions;

public sealed class DataAnnotationsExceptionTests
{
    internal const string DataKey = "SGuard:DataAnnotations";

    [Fact]
    public void DataAnnotationsException_StoresValidationErrorsInDataDictionary()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new("Error 1", new[] { "Property1" }),
            new("Error 2", new[] { "Property2" })
        };

        // Act
        var exception = new DataAnnotationsException(validationResults);

        // Assert
        Assert.True(exception.Data.Contains(DataKey));
        var errors = exception.Data[DataKey] as List<ValidationError>;
        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.Equal("Error 1", errors[0].Message);
        Assert.Contains("Property1", errors[0].Members);
        Assert.Equal("Error 2", errors[1].Message);
        Assert.Contains("Property2", errors[1].Members);
    }

    [Fact]
    public void DataAnnotationsException_HandlesEmptyValidationResults()
    {
        // Arrange
        var validationResults = new List<ValidationResult>();

        // Act
        var exception = new DataAnnotationsException(validationResults);

        // Assert
        Assert.False(exception.Data.Contains(DataKey));
    }

    [Fact]
    public void DataAnnotationsException_UsesCustomMessage()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new("Error 1", new[] { "Property1" })
        };
        const string customMessage = "Custom validation error message.";

        // Act
        var exception = new DataAnnotationsException(customMessage, validationResults);

        // Assert
        Assert.Equal(customMessage, exception.Message);
        Assert.True(exception.Data.Contains(DataKey));
    }

    [Fact]
    public void ValidationError_CreatesRecordWithCorrectValues()
    {
        // Arrange
        const string message = "Validation error occurred.";
        var members = new[] { "Field1", "Field2" };

        // Act
        var validationError = new ValidationError(message, members);

        // Assert
        Assert.Equal(message, validationError.Message);
        Assert.Equal(members, validationError.Members);
    }
}