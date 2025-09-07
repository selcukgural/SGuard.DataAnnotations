using System.ComponentModel.DataAnnotations;

namespace SGuard.DataAnnotations;

public sealed class LocalizedRequiredAttribute : RequiredAttribute
{
    public LocalizedRequiredAttribute(Type resourceType, string resourceName)
    {
        ErrorMessageResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        ErrorMessageResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
    }
}