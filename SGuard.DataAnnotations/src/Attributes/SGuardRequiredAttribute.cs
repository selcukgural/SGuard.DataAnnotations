using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

/// <summary>
/// A custom validation attribute that enforces a required field constraint
/// with support for localized error messages.
/// </summary>
public class SGuardRequiredAttribute : RequiredAttribute
{

}