using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations.Exceptions;

/// <summary>
/// Represents a custom exception for handling validation errors from DataAnnotations.
/// </summary>
public sealed class DataAnnotationsException : ValidationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataAnnotationsException"/> class with a list of validation results and an optional message.
    /// </summary>
    /// <param name="validationResults">The list of validation results containing validation errors.</param>
    /// <param name="message">An optional message describing the exception.</param>
    private DataAnnotationsException(List<ValidationResult> validationResults, string? message = null) : base(message)
    {
        if (validationResults.Count == 0)
        {
            return;
        }

        // Transform validation results into a list of ValidationError records.
        var errors = validationResults.Select(x => new ValidationError(
                                                  Message: string.IsNullOrWhiteSpace(x.ErrorMessage) ? "Errors" : x.ErrorMessage, 
                                                  x.MemberNames))
                                      .ToList();

        // Store the errors and their count in the Data dictionary for later inspection.
        Data["SGuard:DataAnnotations"] = errors;
        Data["SGuard:DataAnnotationsCount"] = errors.Count;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAnnotationsException"/> class with a list of validation results.
    /// </summary>
    /// <param name="validationResults">The list of validation results containing validation errors.</param>
    public DataAnnotationsException(List<ValidationResult> validationResults) 
        : this(validationResults, 
               "DataAnnotations validation failed. Check the Exception.Data[SGuard:DataAnnotations] dictionary key for more details.") 
    { 
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAnnotationsException"/> class with a custom message and a list of validation results.
    /// </summary>
    /// <param name="message">A custom message describing the exception.</param>
    /// <param name="validationResults">The list of validation results containing validation errors.</param>
    public DataAnnotationsException(string message, List<ValidationResult> validationResults) 
        : this(validationResults, message) 
    { 
    }
}

/// <summary>
/// Represents a validation error with a message and the associated members.
/// </summary>
/// <param name="Message">The error message.</param>
/// <param name="Members">The members associated with the validation error.</param>
public sealed record ValidationError(string Message, IEnumerable<string> Members);