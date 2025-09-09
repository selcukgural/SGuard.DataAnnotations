using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SGuard.DataAnnotations;

/// <summary>
/// Base class for localized validation attributes. Provides a mechanism to assign 
/// resource type and resource name for error messages in a DRY (Don't Repeat Yourself) manner.
/// Adds fallback message and fallback resource support.
/// </summary>
public abstract class SGuardValidationAttributeBase : ValidationAttribute
{
    /// <summary>
    /// Gets or sets the name of the fallback resource for the error message.
    /// This is used when the main resource cannot be found.
    /// </summary>
    public string? FallbackResourceName { get; set; }

    /// <summary>
    /// Gets or sets the fallback error message to use if the main resource is not found.
    /// This provides a default error message when localization fails.
    /// </summary>
    public string? FallbackMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SGuardValidationAttributeBase"/> class.
    /// </summary>
    /// <param name="resourceType">The type of the resource file containing the error message.</param>
    /// <param name="resourceName">The name of the resource key for the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourceType"/> or <paramref name="resourceName"/> is null.
    /// </exception>
    protected SGuardValidationAttributeBase(Type resourceType, string resourceName)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }

    /// <summary>
    /// Formats the error message, using the fallback resource or fallback message if the main resource is not found.
    /// </summary>
    /// <param name="name">The name of the field or object being validated.</param>
    /// <returns>The formatted error message.</returns>
    public override string FormatErrorMessage(string name)
    {
        string? message = null;

        try
        {
            message = base.FormatErrorMessage(name);
        }
        catch
        {
            // Ignore exceptions and use fallback mechanisms.
        }

        if (!string.IsNullOrEmpty(message) && !message.StartsWith('[') && !message.Contains("resource", StringComparison.OrdinalIgnoreCase))
        {
            return message;
        }

        if (string.IsNullOrWhiteSpace(FallbackResourceName) || ErrorMessageResourceType == null)
        {
            return !string.IsNullOrWhiteSpace(FallbackMessage) ? FallbackMessage! : $"[{ErrorMessageResourceName}]";
        }

        var prop = ErrorMessageResourceType.GetProperty(FallbackResourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (prop == null)
        {
            return !string.IsNullOrWhiteSpace(FallbackMessage) ? FallbackMessage! : $"[{ErrorMessageResourceName}]";
        }

        var val = prop.GetValue(null, null) as string;

        if (!string.IsNullOrEmpty(val))
        {
            return val;
        }

        return !string.IsNullOrWhiteSpace(FallbackMessage) ? FallbackMessage! : $"[{ErrorMessageResourceName}]";
    }
}