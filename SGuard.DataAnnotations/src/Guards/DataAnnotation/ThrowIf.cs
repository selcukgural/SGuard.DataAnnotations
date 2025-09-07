using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using SGuard.DataAnnotations.Exceptions;
using SGuard.DataAnnotations.Internal;

// ReSharper disable once CheckNamespace
namespace SGuard.DataAnnotations;

/// <summary>
/// Provides exception-throwing guard methods for DataAnnotations validation, following the SGuard pattern.
/// <para>
/// <b>Pattern:</b> All <c>ThrowIf.*</c> methods throw exceptions when validation fails, support custom exception types and constructor arguments, and invoke callbacks with <see cref="GuardOutcome.Failure"/> before throwing and <see cref="GuardOutcome.Success"/> when validation passes.
/// </para>
/// <para>
/// For details, see the SGuard main documentation: https://github.com/selcukgural/sguard
/// </para>
/// </summary>
public static class ThrowIf
{
    /// <summary>
    /// Validates the specified object instance using DataAnnotations and throws an exception if the object is invalid.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="validateAllProperties">
    /// A boolean value indicating whether to validate all properties.
    /// If true, all properties are validated; otherwise, only required properties are validated.
    /// </param>
    /// <param name="callback">
    /// An optional callback to invoke after the validation process.
    /// The callback receives the validation outcome.
    /// </param>
    /// <exception cref="DataAnnotationsException">
    /// Thrown when the object fails validation. The exception contains the validation results.
    /// </exception>
    public static void DataAnnotationsInValid(object instance, bool validateAllProperties = true, SGuardCallback? callback = null)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.Guard(!valid, () => Throw.That(new DataAnnotationsException(results)), callback);
    }

    /// <summary>
    /// Validates the specified object instance using DataAnnotations and throws the provided exception if the object is invalid.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw if validation fails.</typeparam>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="exception">The exception to throw if the object is invalid.</param>
    /// <param name="validateAllProperties">
    /// A boolean value indicating whether to validate all properties.
    /// If true, all properties are validated; otherwise, only required properties are validated.
    /// </param>
    /// <param name="callback">
    /// An optional callback to invoke after the validation process.
    /// The callback receives the validation outcome.
    /// </param>
    /// <exception cref="TException">Thrown when the object fails validation.</exception>
    public static void DataAnnotationsInValid<TException>(object instance, [NotNull]TException exception, bool validateAllProperties = true,
                                                          SGuardCallback? callback = null) where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);
        
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.Guard(!valid, () => Throw.That(exception), callback);
    }

    /// <summary>
    /// Validates the specified object instance using DataAnnotations and throws the provided exception 
    /// if the object is invalid. The exception is created using the specified constructor arguments.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw if validation fails.</typeparam>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="constructorArgs">
    /// An array of arguments to pass to the constructor of the exception being created.
    /// </param>
    /// <param name="validateAllProperties">
    /// A boolean value indicating whether to validate all properties.
    /// If true, all properties are validated; otherwise, only required properties are validated.
    /// </param>
    /// <param name="callback">
    /// An optional callback to invoke after the validation process.
    /// The callback receives the validation outcome.
    /// </param>
    /// <exception cref="TException">Thrown when the object fails validation.</exception>
    public static void DataAnnotationsInValid<TException>(object instance, object[]? constructorArgs, bool validateAllProperties = true,
                                                          SGuardCallback? callback = null) where TException : Exception
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.Guard(!valid, () => Throw.That(ExceptionActivator.Create<TException>(constructorArgs)), callback);
    }
}