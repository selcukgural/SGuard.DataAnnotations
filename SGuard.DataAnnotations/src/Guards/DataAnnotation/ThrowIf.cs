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

        SGuardDataAnnotations.Guard(!valid, () => throw new DataAnnotationsException(results), callback);
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

        SGuardDataAnnotations.Guard(!valid, () => throw exception, callback);
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
    /// <exception cref="InvalidOperationException">
    /// Thrown when the object fails validation and no constructor, or more than one, matches <paramref name="constructorArgs"/>.
    /// </exception>
    [Obsolete(Obsoletions.ConstructorArgsMessage, DiagnosticId = Obsoletions.ConstructorArgsDiagnosticId)]
    public static void DataAnnotationsInValid<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TException>(
        object instance, object[]? constructorArgs, bool validateAllProperties = true, SGuardCallback? callback = null) where TException : Exception
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.Guard(!valid, () => throw SGuardDataAnnotations.CreateException<TException>(constructorArgs), callback);
    }

    /// <summary>
    /// Validates the specified object instance using DataAnnotations and throws the exception created by
    /// <paramref name="exceptionFactory"/> if the object is invalid. The factory runs only when validation fails.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw if validation fails.</typeparam>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="exceptionFactory">Creates the exception to throw, for example <c>() => new MyException("message")</c>.</param>
    /// <param name="validateAllProperties">
    /// A boolean value indicating whether to validate all properties.
    /// If true, all properties are validated; otherwise, only required properties are validated.
    /// </param>
    /// <param name="callback">
    /// An optional callback to invoke after the validation process.
    /// The callback receives the validation outcome.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory"/> is <c>null</c>.</exception>
    /// <exception cref="TException">Thrown when the object fails validation.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the object fails validation and <paramref name="exceptionFactory"/> returns <c>null</c>.
    /// </exception>
    public static void DataAnnotationsInValid<TException>(object instance, Func<TException> exceptionFactory, bool validateAllProperties = true,
                                                          SGuardCallback? callback = null) where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exceptionFactory);

        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.Guard(!valid,
                                    () => throw exceptionFactory() ??
                                                throw new InvalidOperationException("The exception factory returned null."),
                                    callback);
    }
}