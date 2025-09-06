using System.ComponentModel.DataAnnotations;
using SGuard.DataAnnotations.Internal;

// ReSharper disable once CheckNamespace
namespace SGuard.DataAnnotations;

/// <summary>
/// Provides methods to validate objects using DataAnnotations.
/// </summary>
public sealed partial class Is
{
    /// <summary>
    /// Validates the specified object instance using DataAnnotations.
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
    /// <returns>
    /// True if the object is valid; otherwise, false.
    /// </returns>
    public static bool DataAnnotationsValid(object instance, bool validateAllProperties = true, SGuardCallback? callback = null)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.InvokeCallbackSafely(valid, callback);

        return valid;
    }

    /// <summary>
    /// Validates the specified object instance using DataAnnotations and returns the validation results.
    /// </summary>
    /// <param name="instance">The object instance to validate.</param>
    /// <param name="results">
    /// When the method returns, contains the list of validation results. 
    /// This parameter is passed uninitialized.
    /// </param>
    /// <param name="validateAllProperties">
    /// A boolean value indicating whether to validate all properties. 
    /// If true, all properties are validated; otherwise, only required properties are validated.
    /// </param>
    /// <param name="callback">
    /// An optional callback to invoke after the validation process. 
    /// The callback receives the validation outcome.
    /// </param>
    /// <returns>
    /// True if the object is valid; otherwise, false.
    /// </returns>
    public static bool DataAnnotationsValid(object instance, out List<ValidationResult> results, bool validateAllProperties = true,
                                            SGuardCallback? callback = null)
    {
        var context = new ValidationContext(instance);
        results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(instance, context, results, validateAllProperties);

        SGuardDataAnnotations.InvokeCallbackSafely(valid, callback);

        return valid;
    }
}