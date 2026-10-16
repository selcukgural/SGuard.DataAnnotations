namespace SGuard.DataAnnotations.Internal;

/// <summary>
/// Messages and diagnostic IDs of the APIs marked <see cref="ObsoleteAttribute"/>.
/// </summary>
internal static class Obsoletions
{
    public const string ConstructorArgsDiagnosticId = "SGUARDDA001";

    public const string ConstructorArgsMessage =
        "Creating the exception from constructor arguments uses reflection and can't be checked by the compiler. Pass a " +
        "factory instead, for example () => new MyException(\"message\"). This overload will be removed in a future version.";
}
